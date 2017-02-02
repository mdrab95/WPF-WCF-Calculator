using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
[ServiceContract]
public interface IService
{
    [OperationContract]
    double AddNumber(double number1, double number2, Guid myGuid);
    [OperationContract]
    double SubNumber(double number1, double number2, Guid myGuid);
    [OperationContract]
    double MultNumber(double number1, double number2, Guid myGuid);
    [OperationContract]
    double DivNumber(double number1, double number2, Guid myGuid);
    [OperationContract]
    double SignChange(double number1, Guid myGuid);
    [OperationContract]
    List<String> HistoryReport(Guid myGuid);
}
