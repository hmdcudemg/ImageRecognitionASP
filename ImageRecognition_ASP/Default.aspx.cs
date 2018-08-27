using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ImageRecognition_ASP
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txbURL.Text))
            {
                Uri uriResult;
                string filename = "";
                bool result = Uri.TryCreate(txbURL.Text, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                if (uriResult != null && result)
                {
                    filename = System.IO.Path.GetFileName(uriResult.LocalPath);
                }
                GetImageURL(uriResult, filename);
            }
            else
            {
                btnUpload.Attributes.Add("onclick", "document.getElementById('" + fluPicture.ClientID + "').click();");
                if (fluPicture.PostedFile != null && fluPicture.PostedFile.ContentLength > 0)
                {
                    if (IsValidPath())
                    {
                        UpLoadAndDisplay();
                    }
                    else
                    {
                        CustomValidator1.IsValid = false;
                    }
                }
                else
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(Server.MapPath("\\images"));

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                }
            }
        }

        private void GetImageURL(Uri uri, string filename)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(uri, Server.MapPath("/images/" + filename));
            }

            imgPicture.ImageUrl = "~/images/" + filename;
            Label1.Text = filename;

            txbURL.Text = "";

            Run("images/" + filename);
        }

        private void UpLoadAndDisplay()
        {
            string imgName = fluPicture.FileName;
            string imgPath = "images/" + imgName;
            if (fluPicture.PostedFile != null && fluPicture.PostedFile.FileName != "")
            {

                fluPicture.SaveAs(Server.MapPath(imgPath));
                imgPicture.ImageUrl = "~/" + imgPath;
                Label1.Text = imgName;
                txbURL.Text = "";

                Run(imgPath);

            }
            else
            {
                Console.WriteLine("\nInvalid file path");
            }
        }

        protected void ValidateFileSize(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = IsValidPath();
        }

        private bool IsValidPath()
        {
            decimal size = Math.Round(((decimal)fluPicture.PostedFile.ContentLength / (decimal)1024), 2);
            if (size > 4096)
            {
                CustomValidator1.ErrorMessage = "File size must not exceed 4MB.";
                imgPicture.ImageUrl = "";
                txbURL.Text = "";
                details.InnerText = "";
                Label1.Text = "";
                Label2.Text = "";
                return false;
            }
            else
            {
                return true;
            }
        }

        async void Run(string imgPath)
        {
            // Make the REST API call.
            Console.WriteLine("\nWait a moment for the results to appear.\n");
            var result = await MakeAnalysisRequest(imgPath);
            details.InnerText = result;
            var obj = JObject.Parse(result);
            var arr = obj["description"]["captions"];
            var obj2 = JObject.Parse(arr[0].ToString());
            Label2.Text = obj2["text"].ToString();
        }

        // Replace <Subscription Key> with your valid subscription key.
        const string subscriptionKey = "aa6814a165894919b5ab0af341afae9e";

        // You must use the same region in your REST call as you used to
        // get your subscription keys. For example, if you got your
        // subscription keys from westus, replace "westcentralus" in the URL
        // below with "westus".
        //
        // Free trial subscription keys are generated in the westcentralus region.
        // If you use a free trial subscription key, you shouldn't need to change
        // this region.
        const string uriBase = "https://southcentralus.api.cognitive.microsoft.com/vision/v2.0/analyze";

        /// <summary>
        /// Gets the analysis of the specified image file by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file to analyze.</param>
        static async Task<string> MakeAnalysisRequest(string imageFilePath)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                // Request parameters. A third optional parameter is "details".
                string requestParameters =
                    "visualFeatures=Categories,Description,Color";

                // Assemble the URI for the REST API Call.
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                // Request body. Posts a locally stored JPEG image.
                byte[] byteData = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFilePath));

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Make the REST API call.
                    response = await client.PostAsync(uri, content);
                }

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.
                return JToken.Parse(contentString).ToString();
            }
            catch (Exception e)
            {
                return "Error getting description, " + e.Message;
            }
        }
    }
}