using ServerApplication.DataObjects;
using ServerApplication.EDMX;
using System.Collections.Generic;
using System.ServiceModel;

namespace ServerApplication
{
    [ServiceContract]
    public interface IDatabaseService
    {
        // Information providing methods
        
        [OperationContract]
        EmployeeEntity Authorize(string email, byte[] hash);

        [OperationContract]
        EmployeeEntity GetEmployee(int table);

        [OperationContract]
        List<DepartmentEntity> GetDepartments();

        [OperationContract]
        List<PositionEntity> GetPositions();

        [OperationContract]
        List<TypeEntity> GetTypes();

        [OperationContract]
        List<CategoryEntity> GetCategories();

        [OperationContract]
        List<ContractorEntity> GetContractors();

        [OperationContract]
        List<StatusEntity> GetStatuses();

        [OperationContract]
        List<ClaimEntity> GetAllClaims(EmployeeEntity c_table, StatusEntity c_status = null);

        [OperationContract]
        List<ClaimEntity> GetEmployeesClaims(EmployeeEntity c_table);

        // Claim editing methods during its lifecycle

        [OperationContract]
        bool CreateClaim(ClaimEntity claim); // creating claim

        [OperationContract]
        bool ApproveClaim(ClaimEntity claim, int table); // approving claim by dispatcher

        [OperationContract]
        bool SendExecuteClaim(ClaimEntity claim, DepartmentEntity department); // sending on execution by dispatcher

        [OperationContract]
        bool ExecuteClaim(ClaimEntity claim, int executorTable); // accepting claim by executor

        [OperationContract]
        bool FinishClaim(ClaimEntity claim); // finishing work on claim

        [OperationContract]
        bool SetContractorClaim(ClaimEntity claim, ContractorEntity contractor); // OR set contractor and then finish

        [OperationContract]
        bool AcceptClaim(ClaimEntity claim); // accepting work by initiator

        [OperationContract]
        bool DeclineClaim(ClaimEntity claim); // OR declining work by initiator

        [OperationContract]
        bool CloseClaim(ClaimEntity claim); // closing claim finally by dispatcher

        // Personal data editing methods ('Account' tab)

        [OperationContract]
        bool UpdatePassword(int table, byte[] old_hash, byte[] new_hash);

        [OperationContract]
        bool UpdatePhone(int table, decimal phone);

        [OperationContract]
        bool UpdateAddress(int table, string new_address);

        [OperationContract]
        bool UpdateEmail(int table, string email);
    }
}
