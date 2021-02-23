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

function decodeHtml(html) {
	var txt = document.createElement("textarea");
	txt.innerHTML = html;
	return txt.value;
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


// script
; (function ($, window) {

	var intervals = {};
	var removeListener = function (selector) {

		if (intervals[selector]) {

			window.clearInterval(intervals[selector]);
			intervals[selector] = null;
		}
	};
	var found = 'waitUntilExists.found';

	/**
	 * @function
	 * @property {object} jQuery plugin which runs handler function once specified
	 *           element is inserted into the DOM
	 * @param {function|string} handler 
	 *            A function to execute at the time when the element is inserted or 
	 *            string "remove" to remove the listener from the given selector
	 * @param {bool} shouldRunHandlerOnce 
	 *            Optional: if true, handler is unbound after its first invocation
	 * @example jQuery(selector).waitUntilExists(function);
	 */

	$.fn.waitUntilExists = function (handler, shouldRunHandlerOnce, isChild) {

		var selector = this.selector;
		var $this = $(selector);
		var $elements = $this.not(function () { return $(this).data(found); });

		if (handler === 'remove') {

			// Hijack and remove interval immediately if the code requests
			removeListener(selector);
		}
		else {

			// Run the handler on all found elements and mark as found
			$elements.each(handler).data(found, true);

			if (shouldRunHandlerOnce && $this.length) {

				// Element was found, implying the handler already ran for all 
				// matched elements
				removeListener(selector);
			}
			else if (!isChild) {

				// If this is a recurring search or if the target has not yet been 
				// found, create an interval to continue searching for the target
				intervals[selector] = window.setInterval(function () {

					$this.waitUntilExists(handler, shouldRunHandlerOnce, true);
				}, 500);
			}
		}

		return $this;
	};

}(jQuery, window));