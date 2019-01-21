using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace MC.Track.FileValidationAPI
{
    public class FileValidationController : ApiController
    {
        public string connectionString
        {
            get
            {
                return "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename="+ AppDomain.CurrentDomain.BaseDirectory + "App_Data\\MCAPIDB.mdf" + ";Integrated Security=True";
            }
            set { }
        }
        [HttpPost]
        [Route("ValidatePayload")]
        [ResponseType(typeof(DataMatchUploadResponse))]
        public async Task<DataMatchUploadResponse> ValidatePayload([FromBody] DataMatchUploadRequest dataMatchUploadRequest)
        {
            DataMatchUploadResponse _dataMatchUploadResponse = new DataMatchUploadResponse();
            _dataMatchUploadResponse.ResponseHeader = new ResponseHeader(dataMatchUploadRequest.RequestHeader);
            _dataMatchUploadResponse.ResponseDetails = new List<ResponseDetail>();
            foreach (Detail _detail in dataMatchUploadRequest.RequestDetail)
            {
                ResponseDetail _responseDetail = new ResponseDetail(_detail);
                _dataMatchUploadResponse.ResponseDetails.Add(_responseDetail);
            }
            _dataMatchUploadResponse = Validate(_dataMatchUploadResponse);
            if (_dataMatchUploadResponse.ResponseHeader.errorData.Count > 0 || _dataMatchUploadResponse.ResponseHeader.matchStatistics.errorRecords > 0)
            {
                return _dataMatchUploadResponse;
            }
            System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
            string businessId = string.Empty;
            string fileId = string.Empty;
            if (headers.Contains("businessId"))
            {
                businessId = headers.GetValues("businessId").FirstOrDefault();
            }
            if (headers.Contains("businessId"))
            {
                fileId = headers.GetValues("fileId").FirstOrDefault();
            }
            InsertIntoSQL(dataMatchUploadRequest);
            return _dataMatchUploadResponse;
            //return GenerateResponse(await _dataMatchUploadResponse.UploadDataMatchFile(UploadedFile, businessId, fileId));
        }
        
        //[HttpPost]
        //[Route("2")]
        //[ResponseType(typeof(DataMatchUploadResponseBody))]
        //public async Task<DataMatchUploadResponseBody> PostMatchedDataFiles2([FromBody] DataMatchUploadRequestBody UploadedFile)
        //{
        //    DataMatchUploadResponseBody _dMUResponse = new DataMatchUploadResponseBody() { responseheader = new ResponseHeader() };
        //    if (!ModelState.IsValid)
        //    {
        //        _dMUResponse.responseheader.orderid = (UploadedFile.requestheader.orderid == null) ? string.Empty : UploadedFile.requestheader.orderid;
        //        ResponseDetail _responseDetail = new ResponseDetail();
        //        _responseDetail.id = "1";
        //        _responseDetail.requestData = UploadedFile.requestdetail;
        //        _responseDetail.errorData = new List<ErrorData>();

        //        foreach (var field in ModelState.Keys)
        //        {
        //            if (ModelState[field].Errors != null)
        //            {
        //                foreach (var _error in ModelState[field].Errors)
        //                {
        //                    ErrorData _errorData = new ErrorData()
        //                    {
        //                        errorCause = "Invalid Request",
        //                        errorField = field,
        //                        errorExplanation = _error.ErrorMessage,
        //                        errorValidationType = ""
        //                    };
        //                    _responseDetail.errorData.Add(_errorData);
        //                }
        //            }
        //        }
        //        _dMUResponse.responsedetail = new List<ResponseDetail>();
        //        _dMUResponse.responsedetail.Add(_responseDetail);
        //        return _dMUResponse;
        //    }
        //    System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
        //    string businessId = string.Empty;
        //    string fileId = string.Empty;
        //    if (headers.Contains("businessId"))
        //    {
        //        businessId = headers.GetValues("businessId").FirstOrDefault();
        //    }
        //    if (headers.Contains("businessId"))
        //    {
        //        fileId = headers.GetValues("fileId").FirstOrDefault();
        //    }
        //    return _dMUResponse;
        //    //return GenerateResponse(await _dataMatchUploadResponse.UploadDataMatchFile(UploadedFile, businessId, fileId));
        //}
        
        //private DataMatchUploadResponse GenerateResponse(DataMatchUploadResponse response, ModelStateDictionary modelState = null)
        //{
        //    //DataMatchUploadResponse _dataMatchUploadResponse;
        //    //if (response.statusCode == 400)
        //    //    _dataMatchUploadResponse = new DataMatchUploadResponse() {result = "", statusCode = response.statusCode }//Request.CreateResponse(HttpStatusCode.BadRequest, response);

        //    //if (response.statusCode == 409)
        //    //    result = Request.CreateResponse(HttpStatusCode.Conflict, response);

        //    //if (response.statusCode == 202)
        //    //    result = Request.CreateResponse(HttpStatusCode.Accepted, response);

        //    //if (response.statusCode == 500)
        //    //    if (modelState != null)
        //    //    {
        //    //        //if (!modelState.IsValid)
        //    //        //{
        //    //        //    return new DataMatchUploadResponse();
        //    //        //}
        //    //        result = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
        //    //    }
        //    //    else
        //    //    {
        //    //        result = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
        //    //    }

        //    //return result;

        //    return new DataMatchUploadResponse();
        //}
        public DataMatchUploadResponse Validate(DataMatchUploadResponse dataMatchUploadResponse)
        {
            List<ErrorData> _errors = new List<ErrorData>();
            int _totalRecords = dataMatchUploadResponse.ResponseDetails.Count;
            int _errorRecords = 0;
            if (dataMatchUploadResponse.ResponseHeader.errorData == null)
            {
                dataMatchUploadResponse.ResponseHeader.errorData = new List<ErrorData>();
            }
            dataMatchUploadResponse.ResponseHeader.errorData.Clear();
            if (dataMatchUploadResponse.ResponseHeader.orderid == null)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "orderid",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "orderId field is not present",
                    errorValidationType = "MISSING"
                });
                dataMatchUploadResponse.ResponseHeader.orderid = "null";
            }
            else
            {
                if (dataMatchUploadResponse.ResponseHeader.orderid == string.Empty)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "orderid",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in orderId field is not valid",
                        errorValidationType = "INVALID"
                    });
                    dataMatchUploadResponse.ResponseHeader.orderid = "null";
                }
            }

            if (dataMatchUploadResponse.ResponseHeader.ordertype == null)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "orderType",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "orderType field is not present",
                    errorValidationType = "MISSING"
                });
                dataMatchUploadResponse.ResponseHeader.ordertype = "null";
            }
            else
            {
                if (!((dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "new") || 
                    (dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "existing")))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "orderType",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in orderType field is not valid",
                        errorValidationType = "INVALID"
                    });
                }
            }
            if (dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "new" && 
                (GetFromSQL("select ordertype from BLOB where orderid = '" + dataMatchUploadResponse.ResponseHeader.orderid + "'").Rows.Count > 0))
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "orderId",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "value provided in orderId field is duplicate",
                    errorValidationType = "INVALID"
                });
            }
            if (dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "existing" && 
                (GetFromSQL("select ordertype from BLOB where orderid = '" + dataMatchUploadResponse.ResponseHeader.orderid + "'").Rows.Count == 0))
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "orderid",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "value provided in orderId field does not exist",
                    errorValidationType = "MISSING"
                });
            }
            
            if (dataMatchUploadResponse.ResponseHeader.businessid == null)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "businessId",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "businessId field is not present",
                    errorValidationType = "MISSING"
                });
            }
            else
            {
                if (dataMatchUploadResponse.ResponseHeader.businessid == string.Empty)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "businessId",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in businessId field is not valid",
                        errorValidationType = "INVALID"
                    });
                }
            }

            if (dataMatchUploadResponse.ResponseHeader.matchtype == string.Empty)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "matchType",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "value provided in matchType is not valid",
                    errorValidationType = "INVALID"
                });
            }
            
            if (dataMatchUploadResponse.ResponseHeader.noofrecords == null)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "noofRecords",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "noofRecords is not present",
                    errorValidationType = "MISSING"
                });
            }
            else
            {
                if (dataMatchUploadResponse.ResponseHeader.noofrecords == string.Empty)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "noofRecords",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in noofRecords field is not valid",
                        errorValidationType = "INVALID"
                    });
                }
            }

            if (dataMatchUploadResponse.ResponseHeader.email == string.Empty)
            {
                _errors.Add(new ErrorData()
                {
                    errorField = "email",
                    errorCause = "INVALID_REQUEST",
                    errorExplanation = "value provided in email field is not valid",
                    errorValidationType = "INVALID"
                });
            }

            if (_errors.Count > 0)
            {
                foreach (ErrorData _error in _errors)
                {
                    dataMatchUploadResponse.ResponseHeader.errorData.Add(_error);
                }
                dataMatchUploadResponse.ResponseDetails = null;
                return dataMatchUploadResponse;
            }

            for (int i = 0; i < _totalRecords; i++)
            {
                _errors = new List<ErrorData>();
                if (dataMatchUploadResponse.ResponseDetails[i].errorData == null)
                {
                    dataMatchUploadResponse.ResponseDetails[i].errorData = new List<ErrorData>();
                }
                dataMatchUploadResponse.ResponseDetails[i].errorData.Clear();
                if (dataMatchUploadResponse.ResponseDetails[i].id == null)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "id",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "id is not present",
                        errorValidationType = "MISSING"
                    });
                }

                if (dataMatchUploadResponse.ResponseHeader.ordertype == "new" && dataMatchUploadResponse.ResponseDetails[i].id != null && (dataMatchUploadResponse.ResponseDetails.Where(_ => _.id == dataMatchUploadResponse.ResponseDetails[i].id).Count() > 1))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "id",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in id field is duplicate",
                        errorValidationType = "INVALID"
                    });
                }

                if (dataMatchUploadResponse.ResponseHeader.ordertype == "existing" &&
                    GetFromSQL("select * from BLOB where orderid = '" + dataMatchUploadResponse.ResponseHeader.orderid + "' and id = '" + dataMatchUploadResponse.ResponseDetails[i].id + "'").Rows.Count < 1)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "id",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in id field does not exist in the order",
                        errorValidationType = "INVALID"
                    });
                }

                if (dataMatchUploadResponse.ResponseDetails[i].requesttype == null)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "requesttype",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "requestType is not present",
                        errorValidationType = "MISSING"
                    });
                    dataMatchUploadResponse.ResponseDetails[i].requesttype = string.Empty;
                }
                else if (dataMatchUploadResponse.ResponseDetails[i].requesttype == string.Empty)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "requesttype",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in requestType field is not valid",
                        errorValidationType = "INVALID"
                    });
                }

                if ((dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "new") &&
                    ((dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "link") ||
                    (dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "update")))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "requesttype",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in requestType field cannot be 'update' or 'link' ",
                        errorValidationType = "INVALID"
                    });
                }

                if ((dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "existing") &&
                    (dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "update") &&
                    (!(GetFromSQL("select trackid from BLOB where id = '" + dataMatchUploadResponse.ResponseDetails[i].id + "' and trackid ='" + (dataMatchUploadResponse.ResponseDetails[i].trackid??"") +"'").Rows.Count>0)))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "trackid",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in the trackId field for requesting updates does not exist",
                        errorValidationType = "INVALID"
                    });
                }

                //if (((dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "update") ||
                //    (dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "link")) &&
                //    (dataMatchUploadResponse.ResponseDetails[i].trackid == null))
                //{
                //    _errors.Add(new ErrorData()
                //    {
                //        errorField = "trackid",
                //        errorCause = "INVALID_REQUEST",
                //        errorExplanation = "value provided in the trackId field for requesting updates is not for a purchased record",
                //        errorValidationType = "MISSING"
                //    });
                //}

                if (((dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "update") ||
                    (dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "link")) &&
                    (dataMatchUploadResponse.ResponseDetails[i].trackid == null))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "trackid",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "trackId is not present",
                        errorValidationType = "MISSING"
                    });
                }
                else if (((dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "update") ||
                    (dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "link")) &&
                    (dataMatchUploadResponse.ResponseDetails[i].trackid == string.Empty))
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "trackid",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in the trackId field is not valid",
                        errorValidationType = "INVALID"
                    });
                }

                if (dataMatchUploadResponse.ResponseDetails[i].companyname == null)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "companyName",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "companyName is not present",
                        errorValidationType = "MISSING"
                    });
                }
                if (dataMatchUploadResponse.ResponseDetails[i].monitoringType == string.Empty)
                {
                    _errors.Add(new ErrorData()
                    {
                        errorField = "monitoringType",
                        errorCause = "INVALID_REQUEST",
                        errorExplanation = "value provided in monitoring field is not valid",
                        errorValidationType = "INVALID"
                    });
                }
                
                if (dataMatchUploadResponse.ResponseHeader.ordertype.ToLower() == "existing" &&
                    dataMatchUploadResponse.ResponseDetails[i].requesttype.ToLower() == "link")
                {
                    if (dataMatchUploadResponse.ResponseDetails[i].trackid == null)
                    {
                        if (dataMatchUploadResponse.ResponseDetails[i].link != null && dataMatchUploadResponse.ResponseDetails[i].link.compliance.Count > 0 && String.IsNullOrEmpty(dataMatchUploadResponse.ResponseDetails[i].link.compliance[0].referenceId))                            
                        {
                            _errors.Add(new ErrorData()
                            {
                                errorField = "requestType",
                                errorCause = "INVALID_REQUEST",
                                errorExplanation = "trackId or referenceId not present in the request for linking",
                                errorValidationType = "INVALID"
                            });
                        }
                        else if (GetFromSQL("select * from BLOB where orderid = '" + dataMatchUploadResponse.ResponseHeader.orderid + "' and referenceId = '" + dataMatchUploadResponse.ResponseDetails[i].link.compliance[0].referenceId + "'").Rows.Count < 1)
                        {
                            _errors.Add(new ErrorData()
                            {
                                errorField = "referenceId",
                                errorCause = "INVALID_REQUEST",
                                errorExplanation = "value provided in referenceId to link trade directory record to compliance record is not valid",
                                errorValidationType = "INVALID"
                            });
                        }
                    }
                    else
                    {
                        if(
                            (dataMatchUploadResponse.ResponseDetails[i].link.compliance != null) &&
                            (dataMatchUploadResponse.ResponseDetails[i].link.compliance.Count > 0) &&
                            dataMatchUploadResponse.ResponseDetails[i].link.compliance[0].referenceId != null
                            )
                        {
                            _errors.Add(new ErrorData()
                            {
                                errorField = "link",
                                errorCause = "INVALID_REQUEST",
                                errorExplanation = "trackId and referenceId should not be provided in the same request for linking",
                                errorValidationType = "INVALID"
                            });
                        }                        
                        else if (GetFromSQL("select * from BLOB where orderid = '" + dataMatchUploadResponse.ResponseHeader.orderid + "' and linktrackid = '" + dataMatchUploadResponse.ResponseDetails[i].trackid + "'").Rows.Count < 1)
                        {
                            _errors.Add(new ErrorData()
                            {
                                errorField = "trackId",
                                errorCause = "INVALID_REQUEST",
                                errorExplanation = "value provided in trackId field to link a request record to a trade directory record is not valid",
                                errorValidationType = "INVALID"
                            });
                        }
                    }
                }

                if (_errors.Count > 0)
                {
                    dataMatchUploadResponse.ResponseDetails[i].errorData.Concat(_errors);
                    _errorRecords++;
                }
                foreach (ErrorData _errorData in _errors)
                {
                    dataMatchUploadResponse.ResponseDetails[i].errorData.Add(_errorData);
                }
            }
            if (dataMatchUploadResponse.ResponseHeader.matchStatistics == null)
            {
                dataMatchUploadResponse.ResponseHeader.matchStatistics = new matchStatistics();
            }
            dataMatchUploadResponse.ResponseHeader.matchStatistics.totalRecords = _totalRecords;
            dataMatchUploadResponse.ResponseHeader.matchStatistics.errorRecords = _errorRecords;
            return dataMatchUploadResponse;
        }

        public DataTable GetFromSQL(string query)
        {
            DataTable _dataTable = new DataTable();
            SqlConnection _sqlConnection = new SqlConnection(connectionString);
            try
            {
                SqlCommand _sqlCommand = new SqlCommand(query, _sqlConnection);
                SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter(_sqlCommand);
                _sqlConnection.Open();
                _sqlDataAdapter.Fill(_dataTable);
                _sqlConnection.Close();
                _sqlDataAdapter.Dispose();
            }
            catch (Exception _exception)
            {

            }
            finally
            {
                if (_sqlConnection.State != ConnectionState.Closed)
                {
                    _sqlConnection.Close();
                }
            }
            return _dataTable;
        }

        public bool InsertIntoSQL(DataMatchUploadRequest dataMatchUploadRequest)
        {
            SqlConnection _sqlConnection = new SqlConnection(connectionString);
            SqlCommand _sqlCommand;
            try
            {
                foreach (Detail _detail in dataMatchUploadRequest.RequestDetail)
                {
                    Linking _linking = new Linking();
                    Linkcompliance _linkcompliance = new Linkcompliance() { referenceId = "" };
                    _linking.trackid = "";
                    _linking.compliance = new List<Linkcompliance>();
                    _linking.compliance.Add(_linkcompliance);
                    if (_detail.link == null)
                    {
                        _detail.link = _linking;
                    }
                    if (_detail.link.trackid == null)
                    {
                        _detail.link.trackid = "";
                    }
                    if (_detail.link.compliance == null)
                    {
                        _detail.link.compliance = new List<Linkcompliance>();
                        _detail.link.compliance.Add(_linkcompliance);
                    }

                    string _insertQuery = "INSERT INTO [dbo].[BLOB] ([orderid], [ordertype], [businessid], [matchtype], [noofrecords], [email], " +
                        "[id], [requesttype], [trackid], [companyname], [address1], [address2], [address3], [address4], [city], [state], [country], [zip], " +
                        "[phone], [url], [contact], [ein], [tin], [vat], [registrationnumber], [monitoringType], [linktrackid], [referenceid], [CustomFields]) " +
                        "VALUES ( " +
                        " N'" + (dataMatchUploadRequest.RequestHeader.orderid)
                    + "', N'" + (dataMatchUploadRequest.RequestHeader.ordertype)
                    + "', N'" + (dataMatchUploadRequest.RequestHeader.businessid)
                    + "', N'" + (dataMatchUploadRequest.RequestHeader.matchtype)
                    + "', N'" + (dataMatchUploadRequest.RequestHeader.noofrecords)
                    + "', N'" + (dataMatchUploadRequest.RequestHeader.email)
                    + "', N'" + (_detail.id)
                    + "', N'" + (_detail.requesttype)
                    + "', N'" + (_detail.trackid)
                    + "', N'" + (_detail.companyname)
                    + "', N'" + (_detail.address.address1)
                    + "', N'" + (_detail.address.address2)
                    + "', N'" + (_detail.address.address3)
                    + "', N'" + (_detail.address.address4)
                    + "', N'" + (_detail.address.city)
                    + "', N'" + (_detail.address.state)
                    + "', N'" + (_detail.address.country)
                    + "', N'" + (_detail.address.zip)
                    + "', N'" + (_detail.phone)
                    + "', N'" + (_detail.url)
                    + "', N'" + (_detail.contact)
                    + "', N'" + (_detail.ein)
                    + "', N'" + (_detail.tin)
                    + "', N'" + (_detail.vat)
                    + "', N'" + (_detail.registrationnumber)
                    + "', N'" + (_detail.monitoringType)
                    + "', N'" + (_detail.link.trackid)
                    + "', N'" + (_detail.link.compliance[0].referenceId)
                    + "', N'" + (_detail.customfields)
                    + "')";
                    _sqlCommand = new SqlCommand(_insertQuery, _sqlConnection);
                    _sqlConnection.Open();
                    _sqlCommand.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception _exception)
            {
                return false;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
    }
}
