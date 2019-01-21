using Newtonsoft.Json;
using System.Collections.Generic;

namespace MC.Track.FileValidationAPI
{
    public class DataMatchUploadRequest
    {
        public Header RequestHeader { get; set; }

        public IList<Detail> RequestDetail { get; set; }
    }

    public class DataMatchUploadResponse
    {
        public ResponseHeader ResponseHeader { get; set; }

        public IList<ResponseDetail> ResponseDetails { get; set; }
    }

    public class ResponseHeader : Header
    {
        public ResponseHeader(Header header)
        {
            this.orderid = header.orderid;
            this.ordertype = header.ordertype;
            this.businessid = header.businessid;
            this.matchtype = header.matchtype;
            this.noofrecords = header.noofrecords;
            this.email = header.email;
        }
        public matchStatistics matchStatistics { get; set; }
        public List<ErrorData> errorData;
    }
    public class ResponseDetail : Detail
    {
        public ResponseDetail(Detail detail)
        {
            this.id = detail.id;
            this.requesttype = detail.requesttype;
            this.trackid = detail.trackid;
            this.companyname = detail.companyname;
            this.address = detail.address;
            this.phone = detail.phone;
            this.url = detail.url;
            this.contact = detail.contact;
            this.ein = detail.ein;
            this.tin = detail.tin;
            this.vat = detail.vat;
            this.registrationnumber = detail.registrationnumber;
            this.monitoringType = detail.monitoringType;
            this.link = detail.link;
            this.customfields = detail.customfields;
        }
        public List<ErrorData> errorData;
    }
    public class Header
    {
        public string orderid { get; set; }
        public string ordertype { get; set; }
        public string businessid { get; set; }
        public string matchtype { get; set; }
        public string noofrecords { get; set; }
        public string email { get; set; }
    }
    public class matchStatistics
    {
        public int totalRecords { get; set; }
        public int errorRecords { get; set; }
    }
    public class Detail
    {
        public string id { get; set; }
        public string requesttype { get; set; }
        public string trackid { get; set; }
        public string companyname { get; set; }
        public Address address { get; set; }
        public string phone { get; set; }
        public string url { get; set; }
        public string contact { get; set; }
        public string ein { get; set; }
        public string tin { get; set; }
        public string vat { get; set; }
        public string registrationnumber { get; set; }
        public string monitoringType { get; set; }
        public Linking link { get; set; }
        public CustomFields customfields { get; set; }
    }
    public class Address
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address4 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
    }
    public class CustomFields
    {
        public string customfield1 { get; set; }
        public string customfield2 { get; set; }
        public string customfield3 { get; set; }
        public string customfield4 { get; set; }
        public string customfield5 { get; set; }
        public string customfield6 { get; set; }
        public string customfield7 { get; set; }
        public string customfield8 { get; set; }
        public string customfield9 { get; set; }
        public string customfield10 { get; set; }
    }
    public class Linkcompliance
    {
        public string referenceId { get; set; }
    }
    public class Linking
    {
        public string trackid { get; set; }
        public IList<Linkcompliance> compliance { get; set; }
    }
    public class ErrorData
    {
        public ErrorData()
        {
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string errorCause { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string errorExplanation { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string errorField { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string errorValidationType { get; set; }
    }
}
