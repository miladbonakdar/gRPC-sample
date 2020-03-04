using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcSample.Proto;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Client;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GrpcClientFactory.AllowUnencryptedHttp2 = true;
            using var http = GrpcChannel.ForAddress("http://localhost:5001");
            var calculator = http.CreateGrpcService<Calculator.ICalculator>();
            var result = await calculator.MultiplyAsync(new Calculator.MultiplyRequest {X = 12, Y = 4});
            Console.WriteLine(result.Result); // 48

            var clock = http.CreateGrpcService<ITimeService>();
            using var cancel = new CancellationTokenSource(TimeSpan.FromMinutes(1));
            var options = new CallOptions(cancellationToken: cancel.Token);

            try
            {
                await foreach (var time in clock.SubscribeAsync(new CallContext(options)))
                {
                    Console.WriteLine($"The time is now: {time.Time}");
                }
            }
            catch (RpcException)
            {
            }
            catch (TaskCanceledException)
            {
            }
        }
    }
}