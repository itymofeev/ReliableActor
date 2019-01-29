"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var http_1 = require("@angular/common/http");
var ApiService = /** @class */ (function () {
    function ApiService(http) {
        this.http = http;
    }
    ApiService.prototype.startVariableExtraction = function (expression) {
        var body = new http_1.HttpParams().set("expression", expression);
        var headers = new http_1.HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });
        return this.http.post('http://localhost:8663/api/extractvariable', body, { headers: headers, responseType: 'text' });
    };
    return ApiService;
}());
exports.ApiService = ApiService;
//# sourceMappingURL=api.service.js.map