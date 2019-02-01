import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/take';
import 'rxjs/add/operator/takeWhile';
import 'rxjs/add/observable/timer'

@Injectable()
export class ApiService {
  private readonly http: HttpClient;

  constructor(http: HttpClient) {
    this.http = http;
  }

  public startVariableExtraction(expression: string, baseUrl: string): Promise<ViewModels.IExtractedVariables> {
    const body = new HttpParams().set(`expression`, expression);
    const headers = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });

    return new Promise((resolve, reject) => {
      this.http.post(baseUrl + '/api/extractvariable', body, { headers, responseType: 'text' })
               .subscribe(correlationId => resolve(this.tryGetExtractedVariables(correlationId, baseUrl, this.http)), _ => reject());
    });
  }

  private tryGetExtractedVariables(correlationId: string,
                                   baseUrl: string,
                                   http: HttpClient): Promise<ViewModels.IExtractedVariables> {
    const delay = 1000;
    const maxAttempts = 35;
    let isFinished = false;

    return new Promise((resolve, reject) => {
      Observable
        .timer(0, delay)
        .takeWhile((currAttempt) => {
          return !isFinished || currAttempt >= maxAttempts;
        })
        .subscribe(() => {
            http.get<ViewModels.IExtractedVariables>(baseUrl + '/api/extractvariable/' + correlationId).subscribe(
              result => {
                if (!result.isFinished) {
                  return;
                }
                isFinished = true;
                resolve(result);
              });
          },
          _ => reject());
    });
  }

  public substituteVariable(variableToValueEntry: ViewModels.IVariableToValueEntry[], expression: string, baseUrl: string): Promise<string> {
    const body = (new HttpParams()).set(`expression`, expression)
                                   .set(`variableToValueEntry`, JSON.stringify(variableToValueEntry));
    const headers = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });

    return new Promise((resolve, reject) => {
      this.http.put(baseUrl + '/api/substitutevariable', body, { headers, responseType: 'text' })
               .subscribe(substituteExpression => resolve(substituteExpression), _ => reject());
    });
  }
}
