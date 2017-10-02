(function () {
    'use strict';
    angular
        .module('employeeModule')
        .service('employeeService', employeeService);

    employeeService.$inject = ['$http','$q'];

    function employeeService($http,$q) {
        var employee = this;

        employee.getEmployee = getEmployee;
        employee.addEmployee = addEmployee;        
        employee.deleteEmployee = deleteEmployee;

        function getEmployee() {
            return $http({
                method: 'GET',
                url: "http://localhost:13475/api/Employee"
            }).then(function (response) {                
                return response.data;                
            }, function (error) {
                return $q.reject(error.status)
            })
        }

        function addEmployee(newEmployee) {
            return $http({
                method: 'POST',
                url: "http://localhost:13475/api/Employee",
                data: newEmployee
            }).then(function (response) {
                return response.data;
            }, function (error) {
                return $q.reject(error.status)
            })
        }             

        function deleteEmployee(email) {
            return $http({
                method: 'DELETE',
                url: "http://localhost:13475/api/Employee?email=" + email
            }).then(function (response) {
                return response.data;
            }, function (error) {
                return $q.reject(error.status)
            })
        }
    }
})();