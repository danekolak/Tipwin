$.validator.addMethod(
    'validateage',
    function (value, element, params) {
        return Date.parse(value) >= Date.parse(params.minumumdate) && Date.parse(value) <= Date.parse(params.maximumdate);
    });

$.validator.unobtrusive.adapters.add(
    'validateage', ['minumumdate', 'maximumdate'], function (options) {
        var params = {
            minumumdate: options.params.minumumdate,
            maximumdate: options.params.maximumdate
        };
        options.rules['validateage'] = params;
        options.messages['validateage'] = options.message;
    });
