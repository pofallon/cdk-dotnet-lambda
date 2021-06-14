using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using System.IO;

namespace CdkDotnetLambda
{
    public class CdkDotnetLambdaStack : Stack
    {
        internal CdkDotnetLambdaStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            Function fn = new DockerImageFunction(this, "MyFunction", new DockerImageFunctionProps {
                Code = DockerImageCode.FromImageAsset(Path.Combine("src","DotNet5Lambda")),
                
            });
        }
    }
}
