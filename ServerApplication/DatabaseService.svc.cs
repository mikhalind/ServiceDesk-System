using ServerApplication.DataObjects;
using ServerApplication.EDMX;
using System.Collections.Generic;
using System.Linq;

namespace ServerApplication
{
    public class DatabaseService : IDatabaseService
    {
        public EmployeeEntity Authorize(string email, byte[] hash)
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                var employee = claimsEntities.employees.FirstOrDefault(t => t.email == email);
                if (employee == null) return null;
                else return (employee.pass_hash.SequenceEqual(hash) == true)? new EmployeeEntity(employee) : null;
            }
        }

        public List<ClaimEntity> GetAllClaims(EmployeeEntity c_table, StatusEntity c_status = null)
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                List<ClaimEntity> list = new List<ClaimEntity>();

                var collection = (c_status == null) ?
                                      claimsEntities.claims :
                                      claimsEntities.claims.Where(c => c.status == c_status.Code);

                if (c_table.Department == 9)
                {
                    foreach (var item in collection)
                        if (item.dispatcher == null || item.dispatcher == c_table.TableNumber)
                            list.Add(new ClaimEntity(item));
                }

                else
                {
                    if (c_status == null)
                    {
                        foreach (var item in collection)
                            if (item.initiator == c_table.TableNumber || item.executor == c_table.TableNumber || (item.department == c_table.Department && item.status == 3))
                                list.Add(new ClaimEntity(item));
                    }
                    else if (c_status.Code == 3)
                    {
                        foreach (var item in collection)
                            if (item.department == c_table.Department)
                                list.Add(new ClaimEntity(item));
                    }
                    else
                    { 
                        foreach (var item in collection)
                            if (item.initiator == c_table.TableNumber || item.executor == c_table.TableNumber)
                                list.Add(new ClaimEntity(item));
                    }
                }
                return list;
            }
        }

        public EmployeeEntity GetEmployee(int table)
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                var emp = claimsEntities.employees.FirstOrDefault(t => t.table_number == table);
                return (emp != null)? new EmployeeEntity(emp) : null;
            }
        }

        public bool UpdatePhone(int table, decimal phone)
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                var employee = claimsEntities.employees.FirstOrDefault(t => t.table_number == table);
                if (employee == null) return false;
                else
                {
                    employee.phone = phone;
                    claimsEntities.SaveChanges();
                    return true;
                }
            }
        }

        public bool UpdatePassword(int table, byte[] old_hash, byte[] new_hash)
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                var employee = claimsEntities.employees.FirstOrDefault(t => t.table_number == table);
                if (employee == null) return false;
                else if (employee.pass_hash != old_hash) return false;
                else
                {
                    employee.pass_hash = new_hash;
                    return true;
                }
            }
        }

        public List<DepartmentEntity> GetDepartments()
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                List<DepartmentEntity> departments = new List<DepartmentEntity>();
                foreach (var item in claimsEntities.departments)
                    departments.Add(new DepartmentEntity(item));
                return departments;
            }
        }

        public List<PositionEntity> GetPositions()
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                List<PositionEntity> positions = new List<PositionEntity>();
                foreach (var item in claimsEntities.positions)
                    positions.Add(new PositionEntity(item));
                return positions;
            }
        }

        public List<StatusEntity> GetStatuses()
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                List<StatusEntity> statuses = new List<StatusEntity>();
                foreach (var item in claimsEntities.statuses)
                    statuses.Add(new StatusEntity(item));
                return statuses;
            }
        }

        public bool UpdateEmail(int table, string email)
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                var employee = claimsEntities.employees.FirstOrDefault(t => t.table_number == table);
                if (employee == null) return false;
                else
                {
                    employee.email = email;
                    claimsEntities.SaveChanges();
                    return true;
                }
            }
        }

        public List<ContractorEntity> GetContractors()
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                List<ContractorEntity> contractors = new List<ContractorEntity>();
                foreach (var item in claimsEntities.contractors)
                    contractors.Add(new ContractorEntity(item));
                return contractors;
            }
        }

        public bool CreateClaim(ClaimEntity claim)
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                int ID = claimsEntities.claims.Max(i => i.id) + 1;
                var clm = new claim { id = ID, 
                                      initiator = (int)claim.Initiator, 
                                      created_date = System.DateTime.Now, 
                                      category = claim.Category.Code,
                                      description = claim.Description,
                                      theme = claim.Theme, 
                                      status = 1, 
                                      type = claim.ClaimType.ID};
                var res = claimsEntities.claims.Add(clm);
                claimsEntities.SaveChanges();
                if (res == null) return false; 
                else return true;
            }
        }

        public bool ApproveClaim(ClaimEntity claim, int table)
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                var clm = claimsEntities.claims.FirstOrDefault(i => i.id == claim.ID);
                clm.approved_date = System.DateTime.Now;
                clm.status = 2;
                clm.dispatcher = table;
                claimsEntities.SaveChanges();
                return true;
            }
        }

        public bool SendExecuteClaim(ClaimEntity claim, DepartmentEntity department)
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                var clm = claimsEntities.claims.FirstOrDefault(i => i.id == claim.ID);
                clm.department = department.ID;
                clm.status = 3;
                claimsEntities.SaveChanges();
                return true;
            }
        }

        public bool ExecuteClaim(ClaimEntity claim, int executorTable)
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                var clm = claimsEntities.claims.FirstOrDefault(i => i.id == claim.ID);
                clm.executor = executorTable;
                clm.executed_date = System.DateTime.Now;
                clm.status = 4;
                claimsEntities.SaveChanges();
                return true;
            }
        }

        public bool FinishClaim(ClaimEntity claim)
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                var clm = claimsEntities.claims.FirstOrDefault(i => i.id == claim.ID);
                clm.finished_date = System.DateTime.Now;
                clm.status = 5;
                claimsEntities.SaveChanges();
                return true;
            }
        }

        public bool SetContractorClaim(ClaimEntity claim, ContractorEntity contractor)
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                var clm = claimsEntities.claims.FirstOrDefault(i => i.id == claim.ID);
                clm.contractor = contractor.ID;
                claimsEntities.SaveChanges();
                return true;
            }
        }

        public bool AcceptClaim(ClaimEntity claim)
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                var clm = claimsEntities.claims.FirstOrDefault(i => i.id == claim.ID);
                clm.confirmed_date = System.DateTime.Now;
                clm.status = 6;
                claimsEntities.SaveChanges();
                return true;
            }
        }

        public bool DeclineClaim(ClaimEntity claim)
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                var clm = claimsEntities.claims.FirstOrDefault(i => i.id == claim.ID);
                clm.executed_date = null;
                clm.finished_date = null;
                clm.approved_date = null;
                clm.confirmed_date = null;
                clm.contractor = null;
                clm.executor = null;
                clm.status = 1;
                claimsEntities.SaveChanges();
                return true;
            }
        }

        public bool CloseClaim(ClaimEntity claim)
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                var clm = claimsEntities.claims.FirstOrDefault(i => i.id == claim.ID);
                clm.status = 7;
                clm.closed_date = System.DateTime.Now;
                return true;
            }
        }

        public List<TypeEntity> GetTypes()
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                List<TypeEntity> types = new List<TypeEntity>();
                foreach (var item in claimsEntities.types)
                    types.Add(new TypeEntity(item));
                return types;
            }
        }

        public List<CategoryEntity> GetCategories()
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                List<CategoryEntity> categories = new List<CategoryEntity>();
                foreach (var item in claimsEntities.categories)
                    categories.Add(new CategoryEntity(item));
                return categories;
            }
        }

        public List<ClaimEntity> GetEmployeesClaims(EmployeeEntity c_table)
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                List<ClaimEntity> list = new List<ClaimEntity>();

                foreach (var item in claimsEntities.claims.Where(c => c.initiator == c_table.TableNumber))
                    list.Add(new ClaimEntity(item));
                return list;
            }
        }

        public bool UpdateAddress(int table, string new_address)
        {
            using (ClaimsEntities claimsEntities = new ClaimsEntities())
            {
                var employee = claimsEntities.employees.FirstOrDefault(t => t.table_number == table);
                if (employee == null) return false;
                else
                {
                    employee.address = new_address;
                    claimsEntities.SaveChanges();
                    return true;
                }
            }
        }
    }
}
