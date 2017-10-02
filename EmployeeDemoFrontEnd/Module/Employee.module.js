(function () {
    'use strict';
    angular
        .module('employeeModule', ['ngMessages'])
        .run(function () {
            console.log('starting Employee Module.');
        });
})();
