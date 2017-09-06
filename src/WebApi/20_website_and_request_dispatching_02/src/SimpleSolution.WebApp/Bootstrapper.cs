using System.Web.Http;

namespace SimpleSolution.WebApp
{
    public class Bootstrapper
    {
        public static void Init(HttpConfiguration configuration)
        {
            // Note. Since response message generation is out of scope
            // of our test. So I have create an extension method called
            // Request.Text(HttpStatusCode, string) to help you generating
            // a textual response.
        }
    }
}