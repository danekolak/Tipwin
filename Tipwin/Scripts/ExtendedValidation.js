/**
* MVC Unobtrusive Extended Validation Plugin 0.1
*
* Copyright (c) 2014 Noor
*
* Licensed under MIT: http://www.opensource.org/licenses/mit-license.php
*/

function ExtendedValidation() {       
    this.isValid = function (value, element, params) {        
        var otherValue = $('#' + getId(element, params.otherproperty)).val();
        var comparison = params.comparison;
        var datatype = params.datatype;
       
        if ((!value && value !== 0) || (!otherValue && otherValue !== 0)) {            
            return true;
        }
        
        if (datatype == 'date') {
            value = Date.parse(value);
            otherValue = Date.parse(otherValue);
        } else if (datatype == 'number') {
            value = parseFloat(value);
            otherValue = parseFloat(otherValue);
        }

        var rt = false;
        switch (comparison) {
            case "greaterthan":
                rt = value > otherValue;
                break;
            case "greaterthanorequalto":
                rt = value >= otherValue;
                break;
            case "lessthan":
                rt = value < otherValue;
                break;
            case "lessthanorequalto":
                rt = value <= otherValue;
                break;
            case "equalto":
                rt = (value == otherValue);
                break;
            case "notequalto":
                rt = (value != otherValue);
                break;
        }
        
        return rt;
    };

    function getId (a, b) {
        var c = a.id.lastIndexOf("_") + 1;
        var id = a.id.substr(0, c) + b.replace(/\./g, "_");        
        return id;
    }
}

var setOptionRulesForComparison = function (ruleName, options) {
    options.rules[ruleName] = {
        otherproperty: options.params.otherproperty,
        comparison: options.params.comparison,
        datatype: options.params.datatype
    };
    if (options.message)
        options.messages[ruleName] = options.message;
};

jQuery.validator.addMethod('greaterthan', function (value, element, params) {
    return new ExtendedValidation().isValid(value, element, params);
});
jQuery.validator.unobtrusive.adapters.add("greaterthan", ["otherproperty", "comparison", "datatype"], function (options) {
    setOptionRulesForComparison("greaterthan", options);
});

jQuery.validator.addMethod('greaterthanorequalto', function (value, element, params) {
    return new ExtendedValidation().isValid(value, element, params);
});
jQuery.validator.unobtrusive.adapters.add("greaterthanorequalto", ["otherproperty", "comparison", "datatype"], function (options) {
setOptionRulesForComparison("greaterthanorequalto", options);
});
jQuery.validator.addMethod('lessthan', function (value, element, params) {
    return new ExtendedValidation().isValid(value, element, params);
});
jQuery.validator.unobtrusive.adapters.add("lessthan", ["otherproperty", "comparison", "datatype"], function (options) {
setOptionRulesForComparison("lessthan", options);
});
jQuery.validator.addMethod('lessthanorequalto', function (value, element, params) {
    return new ExtendedValidation().isValid(value, element, params);
});
jQuery.validator.unobtrusive.adapters.add("lessthanorequalto", ["otherproperty", "comparison", "datatype"], function (options) {
setOptionRulesForComparison("lessthanorequalto", options);
});
/*
jQuery.validator.addMethod('equalto', function (value, element, params) {
    return new ExtendedValidation().isValid(value, element, params);
});
jQuery.validator.unobtrusive.adapters.add("equalto", ["otherproperty", "comparison", "datatype"], function (options) {
    setOptionRulesForComparison("equalto", options);
});
*/

jQuery.validator.addMethod('notequalto', function (value, element, params) {
    return new ExtendedValidation().isValid(value, element, params);
});
jQuery.validator.unobtrusive.adapters.add("notequalto", ["otherproperty", "comparison", "datatype"], function (options) {
    setOptionRulesForComparison("notequalto", options);
});


