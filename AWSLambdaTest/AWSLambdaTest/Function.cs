using Amazon.Lambda.Core;
using Amazon.Runtime.Internal.Util;
using Amazon.S3;
using Amazon.S3.Util;
using AWS.Logger;
// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]


namespace AWSLambdaTest;

public class Function
{      
    public Response FunctionHandler(Request input, ILambdaContext context)
    {
        
        // Here we can perform any DB operations or any API can or any other operation as per our requirement.

        var number = input.Number;

        context.Logger.LogLine("No Received:" + number); // This logs can be found in cloud watch. 

        var result = number * number;

        context.Logger.LogLine("Square is:" + result);

        return new Response { Result = result };
        // if lambda function is not required to return anything we can simply return null.

    }
}
public class Request
{
    public int Number { get; set; }
}

public class Response
{
    public int Result { get; set; }
}
