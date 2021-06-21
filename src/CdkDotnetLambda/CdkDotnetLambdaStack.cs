using Amazon.CDK;
using Amazon.CDK.AWS.APIGatewayv2;
using Amazon.CDK.AWS.APIGatewayv2.Integrations;
using Amazon.CDK.AWS.CloudWatch;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using System;
using System.IO;

namespace CdkDotnetLambda
{
    public class CdkDotnetLambdaStack : Stack
    {
        internal CdkDotnetLambdaStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            HttpApi httpApi = new HttpApi(this, "HttpApi");

            var dotNet5Function = new DockerImageFunction(this, "DotNet5Function", new DockerImageFunctionProps {
                Code = DockerImageCode.FromImageAsset(Path.Combine("src","DotNet5Lambda")),
                LogRetention = RetentionDays.ONE_WEEK,
                Timeout = Duration.Seconds(60)
            });
            var dotNet5Integration = new LambdaProxyIntegration(new LambdaProxyIntegrationProps {
                Handler = dotNet5Function
            });
            httpApi.AddRoutes(new AddRoutesOptions {
                Path = "/dotnet5",
                Methods = new HttpMethod[] { HttpMethod.GET },
                Integration = dotNet5Integration
            });

            // Function dotNet6Function = new DockerImageFunction(this, "DotNet6Function", new DockerImageFunctionProps {
            //     Code = DockerImageCode.FromImageAsset(Path.Combine("src","DotNet6Lambda")),
            //     LogRetention = RetentionDays.ONE_WEEK,
            //     Timeout = Duration.Seconds(60)
            // });
            // var dotNet6Integration = new LambdaProxyIntegration(new LambdaProxyIntegrationProps {
            //     Handler = dotNet6Function
            // });
            // httpApi.AddRoutes(new AddRoutesOptions {
            //     Path = "/dotnet6",
            //     Methods = new HttpMethod[] { HttpMethod.GET }
            // });

            // var dashboard = new Dashboard(this, "LambdaColdStartDashboard", new DashboardProps {
            //     DashboardName = "Lambda ColdStart Dashboard"
            // });
            // dashboard.AddWidgets()

            var cfnOutput = new CfnOutput(this, "LambdaAPIGatewayEndpoint", new CfnOutputProps {
                Description = "Lambda API Gateway Endpoint",
                ExportName = "lambda-api-gateway-endpoint",
                Value = httpApi.ApiEndpoint
             });
        }
    }
}
