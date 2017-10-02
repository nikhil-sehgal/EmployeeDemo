using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDemoDataAccess;
using EmployeeDemoWebApi.Models;

namespace EmployeeDemoWebApi.Controllers
{
    public class EmployeeController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetUsers()
        {
            try
            {
                using (employeeDBEntities entities = new employeeDBEntities())
                {
                    var query = from emp in entities.tblEmployees
                                join empInfo in entities.tblEmployeeInfoes on emp.Id equals empInfo.Id
                                select new Employee
                                {
                                    Email = emp.Email,
                                    FirstName = empInfo.FirstName,
                                    LastName = empInfo.LastName
                                };
                    return Request.CreateResponse(HttpStatusCode.OK, query.ToList());
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUsersByEmail(string employeeEmail)
        {
            try
            {
                using (employeeDBEntities entities = new employeeDBEntities())
                {
                    var query = from emp in entities.tblEmployees
                                join empInfo in entities.tblEmployeeInfoes on emp.Id equals empInfo.Id
                                where emp.Email == employeeEmail
                                select new Employee
                                {
                                    Email = emp.Email,
                                    FirstName = empInfo.FirstName,
                                    LastName = empInfo.LastName
                                };
                    return Request.CreateResponse(HttpStatusCode.Found, query.ToList());
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e);
            }
        }

        [HttpPost]
        public HttpResponseMessage AddNewUser([FromBody] Employee employee)
        {
            try
            {
                using (var entities = new employeeDBEntities())
                {
                    var entity = entities.tblEmployees.FirstOrDefault(u => u.Email == employee.Email);
                    if (entity != null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.Forbidden,
                             "User with email" + employee.Email.ToString() + " already present.");
                    }
                    else
                    {
                        tblEmployee employeeObj = new tblEmployee
                        {
                            Email = employee.Email
                        };

                        tblEmployeeInfo employeeInfo = new tblEmployeeInfo
                        {

                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                        };

                        employeeInfo.tblEmployee = employeeObj;

                        entities.tblEmployees.Add(employeeObj);
                        entities.tblEmployeeInfoes.Add(employeeInfo);
                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.Created, employee);
                    }
                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateUser([FromBody] Employee employee)
        {
            try
            {
                using (var entities = new employeeDBEntities())
                {
                    var entity = entities.tblEmployees.FirstOrDefault(u => u.Email == employee.Email);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                             "User with email" + employee.Email.ToString() + " not present.");
                    }
                    else
                    {
                        var entityInfo = entity.tblEmployeeInfoes.FirstOrDefault(u => u.Id == entity.Id);
                        entityInfo.FirstName = employee.FirstName;
                        entityInfo.LastName = employee.LastName;
                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, employee);
                    }
                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        [HttpDelete]
        public HttpResponseMessage DeleteUser(String email)
        {
            try
            {
                using (employeeDBEntities entities = new employeeDBEntities())
                {
                    var entity = entities.tblEmployees.FirstOrDefault(e => e.Email == email);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Employee with Id = " + email + " not found to delete");
                    }
                    else
                    {
                        var entityInfo = entity.tblEmployeeInfoes.FirstOrDefault(u => u.Id == entity.Id);
                        entities.tblEmployeeInfoes.Remove(entityInfo);
                        entities.tblEmployees.Remove(entity);
                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
