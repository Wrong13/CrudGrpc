using DbAndGrpc;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("http://localhost:5274");

var client = new UserService.UserServiceClient(channel);

ListReply users = await client.ListUsersAsync(new Google.Protobuf.WellKnownTypes.Empty());
foreach (var user in users.Users)
{
    Console.WriteLine($"{user.Id}. {user.Name} - {user.Age}");
}
