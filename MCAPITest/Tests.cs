using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MC.Track.FileValidationAPI.Tests.Integration
{
    [TestClass]
    public class Tests
    {
        static HttpClient _httpClient;
        StringContent _stringContent;
        HttpResponseMessage response;
        public async void SendPayloadAsync(string jSONPayload, string validationMessage)
        {
            try
            {
                _httpClient = new HttpClient();
                _httpClient.BaseAddress = new Uri("http://localhost:49500/");
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _stringContent = new StringContent(jSONPayload, Encoding.UTF8, "application/json");
                response = new HttpResponseMessage();
                response = _httpClient.PostAsync("ValidatePayload", _stringContent).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                if (!response.Content.ReadAsStringAsync().Result.Contains(validationMessage))
                {
                    Assert.Fail("Expected Validation did not occur");
                }
            }
            catch (Exception _exception)
            {
                Assert.Fail("Exception Thrown" + _exception.Message);
            }
        }

        [TestMethod]
        public void Test1()
        {
            string _jSONPayload = "{\"requestHeader\": {\"fieldId\": \"1\",\"orderType\": \"new\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"premium\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "orderId field is not present";
            this.SendPayloadAsync(_jSONPayload,_validationMessage);
        }

        [TestMethod]
        public void Test2()
        {
            string _jSONPayload = "{\"requestHeader\": {\"fieldId\": \"1\",\"orderType\": \"old\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"premium\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "value provided in orderType field is not valid";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
        }

        [TestMethod]
        public void Test3()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"new\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"premium\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "value provided in orderId field is duplicate";
            //this.SendPayloadAsync(_jSONPayload, "");
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
        }        
    }
}
