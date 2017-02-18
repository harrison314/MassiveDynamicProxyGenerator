using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WcfForHipsters.WebServer.Contract;

namespace WcfForHipsters.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Change url
            Uri endpoint = new Uri("http://localhost:57106/api/ExampleServise");

            ExampleServiceClient client = new ExampleServiceClient(endpoint);

            int sum = client.CalCulateAdd(14, 6, -1);
            Console.WriteLine("Sum is {0}", sum);

            CreateBookRequest createBook = new CreateBookRequest();
            createBook.MarkdawnText = "# Title\nAny text";
            createBook.Title = "Title";
            createBook.Metadata = new RequestMetadata()
            {
                CorelationId = 475,
                Format = ReuquestFormat.Sync,
                Nonce = null
            };

            CreatBookResponse response = client.CreateBook(createBook);

            Console.WriteLine("Book creted with Id {0}", response.Id);
        }
    }
}
