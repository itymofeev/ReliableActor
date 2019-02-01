import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/take';
import 'rxjs/add/operator/takeWhile';
import 'rxjs/add/observable/timer'

@Injectable()
export class ApiService {
  private readonly http: HttpClient;
  private readonly baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  public startVariableExtraction(expression: string): Promise<ViewModels.IExtractedVariables> {
    let body = new HttpParams().set(`expression`, expression);
    let headers = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });

    return new Promise((resolve, reject) => {
      this.http.post(this.baseUrl + 'api/extractvariable', body, { headers, responseType: 'text' })
              .subscribe(correlationId => resolve(this.tryGetExtractedVariables(correlationId, this.http)), _ => reject());
    });
  }

  private tryGetExtractedVariables(correlationId: string,
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
          http.get<ViewModels.IExtractedVariables>(this.baseUrl + 'api/extractvariable/' + correlationId).subscribe(
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

  public substituteVariable(variableToValueEntry: ViewModels.IVariableToValueEntry[], expression: string): Promise<string> {
    const body = (({
      variablesToValuesMap: variableToValueEntry,
      expression: expression
    }) as any) as ViewModels.ISubstitutedVariablesRequest;

    return new Promise((resolve, reject) => {
      this.http.put(this.baseUrl + 'api/substitutevariable', body, { responseType: 'text' })
               .subscribe(substituteExpression => resolve(substituteExpression), _ => reject());
    });
  }
}
