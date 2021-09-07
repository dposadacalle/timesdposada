using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using timesdposada.Common.Responses;
using timesdposada.Common.Models;
using timesdposada.Functions.Entities;

namespace timesdposada.Functions.Functions
{
    public static class TimeApi
    {

        /*
            Daniel Posada
            Date: 22/08/2021
            Methodo: POST
            Description: 
               - Endpoint for create for the employee a new Time Register
        */
        [FunctionName(nameof(CreateEntry))]
        public static async Task<IActionResult> CreateEntry(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "times")] HttpRequest req,
            [Table("times", Connection = "AzureWebJobsStorage")] CloudTable timeTable,
            ILogger log)
        {
            log.LogInformation("Receive a new Time for him Employee.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            TimeEntity timeEntity;

            string message;

            Response resultObject = new Response();

            try
            {
                // Create a varaible time and Deserialize the object send in the Request
                Time time = JsonConvert.DeserializeObject<Time>(requestBody);

                // Create new TimeEntity with, and the send as parameter the propities of the Time
                timeEntity = new TimeEntity
                {
                    EmployeeId = time.EmployeeId,
                    ETag = "*",
                    Date = DateTime.Parse(time.Date.ToString()),
                    Type = time.Type,
                    IsConsolidated = false,
                    PartitionKey = "TIME",
                    RowKey = Guid.NewGuid().ToString()
                };

                TableOperation addOperation = TableOperation.Insert(timeEntity);
                await timeTable.ExecuteAsync(addOperation);

                message = "New Time stored in table.";
                log.LogInformation(message);

                // Varariable resultObject, create a new Object Response 
                resultObject = new Response
                {
                    IsSucess = true,
                    Message = message,
                    Result = timeEntity
                };

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return new OkObjectResult(resultObject);

        }


        /*
          Daniel Posada
          Date: 23/08/2021
          Method: PUT
          Description: Update the entry for the Id 
             
        */
        [FunctionName(nameof(UpdateTime))]
        public static async Task<IActionResult> UpdateTime(
             [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "times/{id}")] HttpRequest req,
             [Table("times", Connection = "AzureWebJobsStorage")] CloudTable entryTable,
             string id,
             ILogger log)
        {
            log.LogInformation($"Update for entry in the time: {id}, received.");

            string message;

            TimeEntity timeEntity;

            Response resultObject = new Response();

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                Time time = JsonConvert.DeserializeObject<Time>(requestBody);

                // Validate Entry Id
                TableOperation findOperation = TableOperation.Retrieve<TimeEntity>("TIME", id);
                TableResult findResult = await entryTable.ExecuteAsync(findOperation);
                if (findResult.Result == null)
                {
                    return new BadRequestObjectResult(new Response
                    {
                        IsSucess = false,
                        Message = "Time not found."
                    });
                }

                timeEntity = (TimeEntity)findResult.Result;
                if (string.IsNullOrEmpty(time?.EmployeeId.ToString()) &&
                    string.IsNullOrEmpty(time?.Date.ToString()) &&
                    string.IsNullOrEmpty(time?.Type.ToString()))
                {
                    timeEntity.Type = time.Type;
                    timeEntity.Date = time.Date;
                    timeEntity.EmployeeId = time.EmployeeId;
                }

                TableOperation addOperation = TableOperation.Replace(timeEntity);
                var updated = await entryTable.ExecuteAsync(addOperation);

                message = $"The Entry: {timeEntity.RowKey} updated in Time Table.";
                log.LogInformation(message);

                resultObject = new Response
                {
                    IsSucess = true,
                    Message = message,
                    Result = timeEntity
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            return new OkObjectResult(resultObject);
        }

        /*
            Daniel Posada
            Date: 23/08/2021
            Methodo: GET
            Description: 
               - Get all the entries from to time table
        */
        [FunctionName(nameof(GetAllTimes))]
        public static async Task<IActionResult> GetAllTimes(

            // Brinding through HttpRequest
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "times")] HttpRequest req,
            // Brinding through CloudTable 
            [Table("times", Connection = "AzureWebJobsStorage")] CloudTable timeTable,
            // Brinding through ILogger
            ILogger log)
        {
            log.LogInformation("Get all entries received.");

            Response resultObject = new Response();

            try
            {
                TableQuery<TimeEntity> query = new TableQuery<TimeEntity>();
                TableQuerySegment<TimeEntity> entrys = await timeTable.ExecuteQuerySegmentedAsync(query, null);

                string message = "Retrieve all entrys.";
                log.LogInformation(message);

                resultObject = new Response
                {
                    IsSucess = true,
                    Message = message,
                    Result = entrys
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            return new OkObjectResult(resultObject);

        }


        /*
            Daniel Posada
            Date: 23/08/2021
            Methodo: DELETE
            Description: 
               - Delete the entry for the Register Id
        */
        [FunctionName(nameof(DeleteTimeById))]
        public static async Task<IActionResult> DeleteTimeById(

             [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "time/{id}")] HttpRequest req,
             [Table("times", "TIME", "{id}", Connection = "AzureWebJobsStorage")] TimeEntity timeEntity,
             [Table("times", Connection = "AzureWebJobsStorage")] CloudTable entryTable,
             string id,
             ILogger log)
        {
            log.LogInformation($"Delete entry: {id} received.");

            Response resultObject = new Response();

            if (timeEntity == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSucess = false,
                    Message = "Entry not found."
                });
            }

            try
            {
                await entryTable.ExecuteAsync(TableOperation.Delete(timeEntity));

                string message = $"Entry {timeEntity.RowKey}, deleted.";
                log.LogInformation(message);

                resultObject = new Response
                {
                    IsSucess = true,
                    Message = message,
                    Result = timeEntity
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            return new OkObjectResult(resultObject);

        }

         /*
             Daniel Posada
             Date: 16/08/2021
             Methodo: GET
             Description: 
                - Get todo by id
         */
        [FunctionName(nameof(GetTimeById))] 
        public static IActionResult GetTimeById(

             [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "time/{id}")] HttpRequest req,
             [Table("times", "TIME", "{id}", Connection = "AzureWebJobsStorage")] TimeEntity timeEntity,
             string id,
             ILogger log)
        {
            log.LogInformation($"Get time by id: {id} received.");


            if (timeEntity == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSucess = false,
                    Message = "Todo not found."
                });
            }

            string message = $"Todo {timeEntity.RowKey}, received.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSucess = true,
                Message = message,
                Result = timeEntity
            });

        }

    }
}
