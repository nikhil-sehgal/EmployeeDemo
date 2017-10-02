(function () {
    'use strict';
    angular
        .module('employeeModule')
        .controller('employeeController', employeeController);


    function employeeController(employeeService) {
        var employeeVm = this;

        employeeVm.headerTitle = "Employee Application";
        employeeVm.listTitle = "Employee List";
        employeeVm.AddNewEmployeeTitle = "Add New Employee";
        employeeVm.getEmployee = getEmployee;
        employeeVm.addEmployee = addEmployee
        employeeVm.deleteEmployee = deleteEmployee;

        init();

        function init() {
            getEmployee();
        }

        function getEmployee() {
            employeeService
                .getEmployee()
                .then(function (data) {
                    employeeVm.employee = data;
                }, function (error) {
                    console.log(error);
                });
        }

        function addEmployee() {
            console.log(employeeVm.newEmployee);
            employeeService
                .addEmployee(employeeVm.newEmployee)
                .then(function (data) {
                    init();
                    employeeVm.newEmployee = {}; // this resets the model
                    //employeeVm.employeeForm.$setPristine(); // this resets the form itself
                }, function (error) {
                    console.log(error);
                });
        }


        function deleteEmployee(email) {
            employeeService
                .deleteEmployee(email)
                .then(function (data) {
                    init();
                }, function (error) {
                    console.log(error);
                });
        }

        return employeeVm;
    }
})();