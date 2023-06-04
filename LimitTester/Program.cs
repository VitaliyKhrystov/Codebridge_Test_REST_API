
namespace LimitTester
{
    internal class Program
    {
        //4. Please implement logic that handles situations when there are too many incoming requests
        //to the application, so those could not be handled.There should be a setting that says how many
        //requests the service can handle, for example, 10 requests per second.In case there are more incoming
        //requests than in configuration application should return HTTP status code "429 Too Many Requests".

        static void Main(string[] args)
        {
            int counter = 0;
            while (counter < 100)
            {
                Parallel.For(0, 8, async r =>
                {
                    using var client = new HttpClient();
                    using var response = await client.GetAsync("https://localhost:44336/ping");
                    Console.WriteLine(response.StatusCode);
                });
                counter++;
            }
        }
    }
}