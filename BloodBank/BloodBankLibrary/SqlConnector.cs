using BloodBankLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace BloodBankLibrary
{
    public class SqlConnector
    {
        private const string connectionStringName = "BloodBank";


        public event EventHandler<DoctorModel> OnDoctorCreated;
        public event EventHandler<DoctorModel> OnDoctorUpdated;
        public event EventHandler<DoctorModel> OnDoctorDeleted;

        public event EventHandler<DonorModel> OnDonorCreated;
        public event EventHandler<DonorModel> OnDonorUpdated;
        public event EventHandler<DonorModel> OnDonorDeleted;

        public event EventHandler<RecipientModel> OnRecipientCreated;
        public event EventHandler<RecipientModel> OnRecipientUpdated;
        public event EventHandler<RecipientModel> OnRecipientDeleted;

        public event EventHandler<RoleModel> OnRoleCreated;
        public event EventHandler<RoleModel> OnRoleDeleted;
        public event EventHandler<RoleModel> OnRoleUpdated;

        public event EventHandler<BloodModel> OnBloodCreated;
        public event EventHandler<BloodModel> OnBloodDeleted;

        public event EventHandler<IssueModel> OnIssueCreated;
        public event EventHandler<IssueModel> OnIssueDeleted;

        public event EventHandler<Dictionary<int,double>> OnBloodAmountUpdated;


        //Roles
        public List<RoleModel> GetAllRoles()
        {
            List<RoleModel> roles = new List<RoleModel>();
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                roles = connection.Query<RoleModel>("dbo.spRoles_GetAll", commandType: CommandType.StoredProcedure).ToList();
            }
            return roles;
        }

        public void CreateRole(RoleModel roleModel)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Name", roleModel.Name);
                parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spRoles_Insert", parameters, commandType: CommandType.StoredProcedure);
                roleModel.Id = parameters.Get<int>("@Id");
            }
            OnRoleCreated?.Invoke(this, roleModel);
        }

        public void UpdateRole(RoleModel roleModel)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Name", roleModel.Name);
                parameters.Add("@Id", roleModel.Id);
                connection.Execute("dbo.spRoles_UpdateById", parameters, commandType: CommandType.StoredProcedure);
            }
            OnRoleUpdated?.Invoke(this,roleModel);
        }

        public void DeleteRole(RoleModel roleModel)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", roleModel.Id);
                connection.Execute("dbo.spRoles_DeleteById", parameters, commandType: CommandType.StoredProcedure);
            }
            OnRoleDeleted?.Invoke(this, roleModel);
        }


        //Doctors
        public List<DoctorModel> GetAllDoctors()
        {
            List<DoctorModel> doctors = new List<DoctorModel>();
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                doctors = connection.Query<DoctorModel>("dbo.spDoctors_GetAll", commandType: CommandType.StoredProcedure).ToList();

                DynamicParameters parameters;
                foreach (var doctor in doctors)
                {
                    parameters = new DynamicParameters();
                    parameters.Add("@DoctorId", doctor.Id);

                    doctor.Role = connection.Query<RoleModel>("dbo.spRoles_GetByDoctorId", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            return doctors;
        }

        public void CreateDoctor(DoctorModel doctorModel)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Name", doctorModel.Name);
                parameters.Add("@Photo", doctorModel.Photo);
                parameters.Add("@RoleId", doctorModel.Role.Id);
                parameters.Add("@Education", doctorModel.Education);
                parameters.Add("@DateOfBirth", doctorModel.DateOfBirth);
                parameters.Add("@Phone", doctorModel.Phone);
                parameters.Add("@Email", doctorModel.Email);
                parameters.Add("@Address", doctorModel.Address);
                parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spDoctors_Insert", parameters, commandType: CommandType.StoredProcedure);
                doctorModel.Id = parameters.Get<int>("@Id");
            }
            OnDoctorCreated?.Invoke(this, doctorModel);
        }

        public void UpdateDoctor(DoctorModel doctorModel)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Name", doctorModel.Name);
                parameters.Add("@Photo", doctorModel.Photo);
                parameters.Add("@RoleId", doctorModel.Role.Id);
                parameters.Add("@Education", doctorModel.Education);
                parameters.Add("@DateOfBirth", doctorModel.DateOfBirth);
                parameters.Add("@Phone", doctorModel.Phone);
                parameters.Add("@Email", doctorModel.Email);
                parameters.Add("@Address", doctorModel.Address);
                parameters.Add("@Id", doctorModel.Id);
                connection.Execute("dbo.spDoctors_UpdateById", parameters, commandType: CommandType.StoredProcedure);
            }
            OnDoctorUpdated?.Invoke(this, doctorModel);
        }

        public void DeleteDoctor(DoctorModel doctorModel)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", doctorModel.Id);
                connection.Execute("dbo.spDoctors_DeleteById", parameters, commandType: CommandType.StoredProcedure);
            }
            OnDoctorDeleted?.Invoke(this, doctorModel);
        }


        //Recipients
        public List<RecipientModel> GetAllRecipients()
        {
            List<RecipientModel> recipients = new List<RecipientModel>();
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                recipients = connection.Query<RecipientModel>("dbo.spRecipients_GetAll", commandType: CommandType.StoredProcedure).ToList();
            }
            return recipients;
        }

        public void CreateRecipient(RecipientModel recipientModel)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Name", recipientModel.Name);
                parameters.Add("@Photo", recipientModel.Photo);
                parameters.Add("@BloodGroup", recipientModel.BloodGroup);
                parameters.Add("@DateOfBirth", recipientModel.DateOfBirth);
                parameters.Add("@Phone", recipientModel.Phone);
                parameters.Add("@Email", recipientModel.Email);
                parameters.Add("@Address", recipientModel.Address);
                parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spRecipients_Insert", parameters, commandType: CommandType.StoredProcedure);
                recipientModel.Id = parameters.Get<int>("@Id");
            }
            OnRecipientCreated?.Invoke(this, recipientModel);
        }

        public void UpdateRecipient(RecipientModel recipientModel)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Name", recipientModel.Name);
                parameters.Add("@Photo", recipientModel.Photo);
                parameters.Add("@BloodGroup", recipientModel.BloodGroup);
                parameters.Add("@DateOfBirth", recipientModel.DateOfBirth);
                parameters.Add("@Phone", recipientModel.Phone);
                parameters.Add("@Email", recipientModel.Email);
                parameters.Add("@Address", recipientModel.Address);
                parameters.Add("@Id", recipientModel.Id);
                connection.Execute("dbo.spRecipients_UpdateById", parameters, commandType: CommandType.StoredProcedure);
            }
            OnRecipientUpdated?.Invoke(this, recipientModel);
        }

        public void DeleteRecipient(RecipientModel recipientModel)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", recipientModel.Id);
                connection.Execute("dbo.spRecipients_DeleteById", parameters, commandType: CommandType.StoredProcedure);
            }
            OnRecipientDeleted?.Invoke(this, recipientModel);
        }


        //Donors
        public List<DonorModel> GetAllDonors()
        {
            List<DonorModel> donors = new List<DonorModel>();
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                donors = connection.Query<DonorModel>("dbo.spDonors_GetAll", commandType: CommandType.StoredProcedure).ToList();
            }
            return donors;
        }

        public void CreateDonor(DonorModel donorModel)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Name", donorModel.Name);
                parameters.Add("@Photo", donorModel.Photo);
                parameters.Add("@BloodGroup", donorModel.BloodGroup);
                parameters.Add("@DateOfBirth", donorModel.DateOfBirth);
                parameters.Add("@Phone", donorModel.Phone);
                parameters.Add("@Email", donorModel.Email);
                parameters.Add("@Address", donorModel.Address);
                parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spDonors_Insert", parameters, commandType: CommandType.StoredProcedure);
                donorModel.Id = parameters.Get<int>("@Id");
            }
            OnDonorCreated?.Invoke(this, donorModel);
        }

        public void UpdateDonor(DonorModel donorModel)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Name", donorModel.Name);
                parameters.Add("@Photo", donorModel.Photo);
                parameters.Add("@BloodGroup", donorModel.BloodGroup);
                parameters.Add("@DateOfBirth", donorModel.DateOfBirth);
                parameters.Add("@Phone", donorModel.Phone);
                parameters.Add("@Email", donorModel.Email);
                parameters.Add("@Address", donorModel.Address);
                parameters.Add("@Id", donorModel.Id);
                connection.Execute("dbo.spDonors_UpdateById", parameters, commandType: CommandType.StoredProcedure);
            }
            OnDonorUpdated?.Invoke(this, donorModel);
        }

        public void DeleteDonor(DonorModel donorModel)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", donorModel.Id);
                connection.Execute("dbo.spDonors_DeleteById", parameters, commandType: CommandType.StoredProcedure);
            }
            OnDonorDeleted?.Invoke(this, donorModel);
        }


        //Blood
        public List<BloodModel> GetBloodCollection()
        {
            List<BloodModel> bloodCollection = new List<BloodModel>();
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                bloodCollection = connection.Query<BloodModel>("dbo.spBloodCollection_GetAll", commandType: CommandType.StoredProcedure).ToList();

                DynamicParameters parameters;
                foreach (var blood in bloodCollection)
                {
                    parameters = new DynamicParameters();
                    parameters.Add("@BloodId", blood.Id);

                    blood.Donor = connection.Query<DonorModel>("dbo.spDonors_GetByBloodId", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    blood.DoctorInCharge = connection.Query<DoctorModel>("dbo.spDoctors_GetByBloodId", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            return bloodCollection;
        }

        public void CreateBlood(BloodModel bloodModel)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DonationType",bloodModel.DonationType);
                parameters.Add("@DonorId", bloodModel.Donor.Id);
                parameters.Add("@BloodGroup", bloodModel.BloodGroup);
                parameters.Add("@Amount", bloodModel.Amount);
                parameters.Add("@Unit", bloodModel.Unit);
                parameters.Add("@DateOfCollection", bloodModel.DateOfCollection);
                parameters.Add("@DoctorInChargeId", bloodModel.DoctorInCharge.Id);
                parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spBloodCollection_Insert", parameters, commandType: CommandType.StoredProcedure);
                bloodModel.Id = parameters.Get<int>("@Id");
            }
            OnBloodCreated?.Invoke(this, bloodModel);
        }

        public void UpdateBlood(BloodModel bloodModel)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DonationType", bloodModel.DonationType);
                parameters.Add("@DonorId", bloodModel.Donor.Id);
                parameters.Add("@BloodGroup", bloodModel.BloodGroup);
                parameters.Add("@Amount", bloodModel.Amount);
                parameters.Add("@Unit", bloodModel.Unit);
                parameters.Add("@DateOfCollection", bloodModel.DateOfCollection);
                parameters.Add("@DoctorInChargeId", bloodModel.DoctorInCharge.Id);
                parameters.Add("@Id", bloodModel.Id);
                connection.Execute("dbo.spBloodCollection_UpdateById", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteBlood(BloodModel bloodModel)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", bloodModel.Id);
                connection.Execute("dbo.spBloodCollection_DeleteById", parameters, commandType: CommandType.StoredProcedure);
            }
            OnBloodDeleted?.Invoke(this, bloodModel);
        }


        //Issue
        public List<IssueModel> GetAllIssues()
        {
            List<IssueModel> issues = new List<IssueModel>();
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                issues = connection.Query<IssueModel>("dbo.spIssue_GetAll", commandType: CommandType.StoredProcedure).ToList();

                DynamicParameters parameters;
                foreach (var issue in issues)
                {
                    parameters = new DynamicParameters();
                    parameters.Add("@IssueId", issue.Id);

                    issue.Recipient = connection.Query<RecipientModel>("dbo.spRecipients_GetByIssueId", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    issue.DoctorInCharge = connection.Query<DoctorModel>("dbo.spDoctors_GetByIssueId", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    issue.Blood = connection.Query<BloodModel>("dbo.spBloodCollection_GetByIssueId", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();


                    parameters = new DynamicParameters();
                    parameters.Add("@BloodId", issue.Blood.Id);

                    issue.Blood.Donor = connection.Query<DonorModel>("dbo.spDonors_GetByBloodId", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            return issues;
        }

        public void CreateIssue(IssueModel issueModel)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@RecipientId",issueModel.Recipient.Id);
                parameters.Add("@BloodId", issueModel.Blood.Id);
                parameters.Add("@BloodAmount", issueModel.BloodAmount);
                parameters.Add("@Unit", issueModel.Unit);
                parameters.Add("@PricePaid", issueModel.PricePaid);
                parameters.Add("@DoctorInChargeId", issueModel.DoctorInCharge.Id);
                parameters.Add("@DateOfIssue", issueModel.DateOfIssue);
                parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spIssue_Insert", parameters, commandType: CommandType.StoredProcedure);
                issueModel.Id = parameters.Get<int>("@Id");
            }
            OnIssueCreated?.Invoke(this, issueModel);
        }

        public void UpdateIssue(IssueModel issueModel)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@RecipientId", issueModel.Recipient.Id);
                parameters.Add("@BloodId", issueModel.Blood.Id);
                parameters.Add("@BloodAmount", issueModel.BloodAmount);
                parameters.Add("@Unit", issueModel.Unit);
                parameters.Add("@PricePaid", issueModel.PricePaid);
                parameters.Add("@DoctorInChargeId", issueModel.DoctorInCharge.Id);
                parameters.Add("@DateOfIssue", issueModel.DateOfIssue);
                parameters.Add("@Id", issueModel.Id);
                connection.Execute("dbo.spIssue_UpdateById", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteIssue(IssueModel issueModel)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", issueModel.Id);
                connection.Execute("dbo.spIssue_DeleteById", parameters, commandType: CommandType.StoredProcedure);
            }
            OnIssueDeleted?.Invoke(this, issueModel);
        }


        //Blood Amount
        public void UpdateBloodAmount(double amount, int bloodId)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Amount", amount);
                parameters.Add("@Id", bloodId);
                connection.Execute("dbo.spBloodCollection_UpdateAmountById", parameters, commandType: CommandType.StoredProcedure);
            }
            OnBloodAmountUpdated?.Invoke(this, new Dictionary<int, double> { { bloodId,amount} });
        }

        public void UpdateIssueBloodAmount(double amount, int issueId)
        {
            using (IDbConnection connection = new SqlConnection(Connector.GetConnectionString(connectionStringName)))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Amount", amount);
                parameters.Add("@Id", issueId);
                connection.Execute("dbo.spIssue_UpdateBloodAmountById", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
