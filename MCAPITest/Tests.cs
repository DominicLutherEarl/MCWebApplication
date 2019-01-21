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
        bool _isTrue = false;
        public void SendPayloadAsync(string jSONPayload, string validationMessage)
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
                    _isTrue = false;
                }
                else
                {
                    _isTrue = true;
                }
            }
            catch (Exception _exception)
            {
                _isTrue = false;
            }
        }

        [TestMethod]
        public void Validation01()
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
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation02()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"\",\"orderType\": \"old\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"premium\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "value provided in orderId field is not valid";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }
        
        [TestMethod]
        public void Validation03()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"premium\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "orderType field is not present";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation04()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"premium\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "value provided in orderType field is not valid";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation05()
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
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation06()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \""+DateTime.Now.Ticks.ToString()+"\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"premium\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "value provided in orderId field does not exist";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation07()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"premium\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "businessId field is not present";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation08()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"premium\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "value provided in businessId field is not valid";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation09()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"premium\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "value provided in matchType is not valid";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation10()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"premium\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "noofRecords is not present";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation11()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"premium\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "value provided in noofRecords field is not valid";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation12()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"premium\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "value provided in email field is not valid";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation13()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"requestType\": \"premium\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "id is not present";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation14()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \""+DateTime.Now.Ticks.ToString()+"\",\"orderType\": \"new\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"premium\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}, {\"id\": \"1\",\"requestType\": \"premium\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "value provided in id field is duplicate";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation15()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \""+DateTime.Now.Ticks.ToString()+"\",\"requestType\": \"premium\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "value provided in id field does not exist in the order";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation16()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "requestType is not present";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation17()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "value provided in requestType field is not valid";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation18()
        {
            bool _flag = false;
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \""+DateTime.Now.Ticks.ToString()+"\",\"orderType\": \"new\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"link\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "value provided in requestType field cannot be 'update' or 'link'";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            _flag = _isTrue;
            _jSONPayload = "{\"requestHeader\": {\"orderId\": \""+DateTime.Now.Ticks.ToString()+"\",\"orderType\": \"new\",\"businessId\": \"US2018000000023\"," +
        "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"update\"," +
        "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
        "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
        "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
        "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
        "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
        "\"customField10\": \"\"}}]}";
            _validationMessage = "value provided in requestType field cannot be 'update' or 'link'";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_flag && _isTrue);
        }

        [TestMethod]
        public void Validation19()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \""+DateTime.Now.Ticks.ToString()+"\",\"requestType\": \"update\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "value provided in the trackId field for requesting updates does not exist";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation20()
        {
            bool _flag = false;
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"update\"," +
                    "\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "trackId is not present";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            _flag = _isTrue;
            _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
        "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"link\"," +
        "\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
        "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
        "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
        "\"link\": {\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
        "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
        "\"customField10\": \"\"}}]}";
            _validationMessage = "trackId is not present";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_flag && _isTrue);
        }

        [TestMethod]
        public void Validation21()
        {
            bool _flag = false;
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"update\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "value provided in the trackId field is not valid";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            _flag = _isTrue;
            _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
        "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"link\"," +
        "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
        "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
        "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
        "\"link\": {\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
        "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
        "\"customField10\": \"\"}}]}";
            _validationMessage = "value provided in the trackId field is not valid";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_flag && _isTrue);
        }

        [TestMethod]
        public void Validation22()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"premium\"," +
                    "\"trackId\": \"\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "companyName is not present";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation23()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"premium\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"\"," +
                    "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "value provided in monitoring field is not valid";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation24()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"link\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \"01\",\"compliance\": [{\"referenceId\": \"11223344\"},{\"referenceId\": \"22334455\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "trackId and referenceId should not be provided in the same request for linking";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation25()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"link\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"trackId\": \""+DateTime.Now.Ticks.ToString()+"\",\"compliance\": [{}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "value provided in trackId field is not valid";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation26()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"link\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"compliance\": [{\"referenceId\": \""+DateTime.Now.Ticks.ToString()+"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "value provided in referenceId to link trade directory record to compliance record is not valid";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        [TestMethod]
        public void Validation27()
        {
            string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
                    "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"link\"," +
                    "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
                    "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
                    "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
                    "\"link\": {\"compliance\": [{}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
                    "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
                    "\"customField10\": \"\"}}]}";
            string _validationMessage = "trackId or referenceId not present in the request for linking";
            this.SendPayloadAsync(_jSONPayload, _validationMessage);
            Assert.IsTrue(_isTrue);
        }

        //[TestMethod]
        //public void Validation13()
        //{
        //    string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
        //            "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"premium\"," +
        //            "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
        //            "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
        //            "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
        //            "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
        //            "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
        //            "\"customField10\": \"\"}}]}";
        //    string _validationMessage = "value provided in orderId field is not valid";
        //    this.SendPayloadAsync(_jSONPayload, _validationMessage);
        //    Assert.IsTrue(_isTrue);
        //}

        //[TestMethod]
        //public void Validation13()
        //{
        //    string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
        //            "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"premium\"," +
        //            "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
        //            "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
        //            "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
        //            "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
        //            "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
        //            "\"customField10\": \"\"}}]}";
        //    string _validationMessage = "value provided in orderId field is not valid";
        //    this.SendPayloadAsync(_jSONPayload, _validationMessage);
        //    Assert.IsTrue(_isTrue);
        //}

        //[TestMethod]
        //public void Validation13()
        //{
        //    string _jSONPayload = "{\"requestHeader\": {\"orderId\": \"1\",\"orderType\": \"existing\",\"businessId\": \"US2018000000023\"," +
        //            "\"matchType\": \"highconfidence\",\"noofRecords\": \"1\",\"email\": \"sampleops@company.com\"},\"requestDetail\": [{\"id\": \"1\",\"requestType\": \"premium\"," +
        //            "\"trackId\": \"\",\"companyName\": \"Mastercard\",\"address\": {\"address1\": \"2200 Mastercard Blvd\",\"address2\": \"\",\"address3\": \"\",\"address4\": \"\"," +
        //            "\"city\": \"O Fallon\",\"state\": \"MO\",\"country\": \"US\",\"zip\": \"63368\"},\"phone\": \"636.722.6100\",\"url\": \"https://www.mastercard.us\"," +
        //            "\"contact\": \"Allen Brewer\",\"ein\": \"13-4172551\",\"tin\": \"\",\"vat\": \"\",\"registrationNumber\": \"13-4172551\",\"monitoringType\": \"manual\"," +
        //            "\"link\": {\"trackId\": \"\",\"compliance\": [{\"referenceId\": \"\"},{\"referenceId\": \"\"}]},\"customFields\": {\"customField1\": \"\",\"customField2\": \"\"," +
        //            "\"customField3\": \"\",\"customField4\": \"\",\"customField5\": \"\",\"customField6\": \"\",\"customField7\": \"\",\"customField8\": \"\",\"customField9\": \"\"," +
        //            "\"customField10\": \"\"}}]}";
        //    string _validationMessage = "value provided in orderId field is not valid";
        //    this.SendPayloadAsync(_jSONPayload, _validationMessage);
        //    Assert.IsTrue(_isTrue);
        //}
    }
}