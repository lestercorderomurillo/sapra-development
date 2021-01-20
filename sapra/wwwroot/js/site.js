function displayLayout() {
	$('#navigation').show();
	$('#footer').show();
}

function ajax(action, model, modelname, div) {
	var dataToSend = model;
	$.ajax({
		type: "post",
		url: action,
		dataType: "text",
		contentType: "application/x-www-form-urlencoded; charset=utf-8",
		data: modelname + "=" + JSON.stringify(dataToSend),
		success: function (response) {
			$(div).html(response);
		},
		error: function (xhr, status) {
			alert(status);
		},
	});
}

function ajax(action, div) {
	$.ajax({
		type: "post",
		url: action,
		success: function (viewResult) {
			if (div != "") {
				$(div).html(viewResult);
			}
		}
	});
}

function minmax(value, min, max) {
	if (parseInt(value) < min || isNaN(parseInt(value)))
		return min;
	else if (parseInt(value) > max)
		return max;
	else return value;
}

function hide(obj) {
	$(obj).hide();
}

function show(obj) {
	$(obj).show();
}

$.validator.addMethod("lessThan", function (value, element, params) {
	if (!/Invalid|NaN/.test(new Date(value))) {
		return new Date(value) < new Date($(params).val());
	}
	return isNaN(value) && isNaN($(params).val())
		|| (Number(value) < Number($(params).val()));
}, 'La fecha debe ser inferior a {0}.');

function resetFormValidator(formId) {
	$(formId).removeData('validator');
	$(formId).removeData('unobtrusiveValidation');
	$.validator.unobtrusive.parse(formId);
}