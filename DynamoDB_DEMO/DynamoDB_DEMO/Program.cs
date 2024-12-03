using System;
using System.Collections.Generic;
using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using System.Configuration;

namespace DynamoDB_DEMO
{
    class Program
    {
        static void Main()
        {
            string accessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
            string secretKey = ConfigurationManager.AppSettings["AWSSecretKey"];

            // Initialize the AmazonDynamoDBClient with credentials
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var config = new AmazonDynamoDBConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast1 // Specify the region you are working in
            };
            var client = new AmazonDynamoDBClient(credentials, config);
            // Configure the Amazon DynamoDB client

            // Table name
            string tableName = "tbl_DynamoDemo";

            // Create the DynamoDB table
            CreateTable(client, tableName);
            
            // Insert operation
            InsertItem(client, tableName);

            // Read operation
            ReadItem(client, tableName);

            // Update operation
            UpdateItem(client, tableName);

            // Read operation after update
            ReadItem(client, tableName);

            // Delete operation
            DeleteItem(client, tableName);
        }
        static void CreateTable(AmazonDynamoDBClient client, string tableName)
        {
            CreateTableRequest request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition
                {
                    AttributeName = "UserId",
                    AttributeType = "S"
                    // string ("S"),
                    // number ("N"),
                    // binary ("B"),
                    // string set ("SS"),
                    // number set ("NS"),
                    // and binary set ("BS")
                }
            },
                KeySchema = new List<KeySchemaElement>
            {
                new KeySchemaElement
                {
                    AttributeName = "UserId",
                    KeyType = "HASH"
                }
            },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                }
            };

            CreateTableResponse response = client.CreateTable(request);
            Console.WriteLine("Table created successfully.");
        }
        static void InsertItem(AmazonDynamoDBClient client, string tableName)
        {
            Table table = Table.LoadTable(client, tableName);

            var item = new Document();
            item["UserId"] = "user123";
            item["Name"] = "John Doe";
            item["Age"] = 30;
            item["Email"] = "john@example.com";

            table.PutItem(item);
            Console.WriteLine("Item inserted successfully.");
        }
        static void ReadItem(AmazonDynamoDBClient client, string tableName)
        {
            Table table = Table.LoadTable(client, tableName);

            var search = table.GetItem("user123");
            if (search != null)
            {
                Console.WriteLine($"Name: {search["Name"]}, Age: {search["Age"]}, Email: {search["Email"]}");
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
        }
        static void UpdateItem(AmazonDynamoDBClient client, string tableName)
        {
            Table table = Table.LoadTable(client, tableName);

            var item = new Document();
            item["UserId"] = "user123";
            item["Name"] = "John Updated";
            item["Age"] = 31;
            item["Email"] = "john_updated@example.com";

            table.PutItem(item);
            Console.WriteLine("Item updated successfully.");
        }
        static void DeleteItem(AmazonDynamoDBClient client, string tableName)
        {
            Table table = Table.LoadTable(client, tableName);

            table.DeleteItem("user123");
            Console.WriteLine("Item deleted successfully.");
        }

    }
}
