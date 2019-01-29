namespace ViewModels {
  export interface ISubstitutedVariables {
    variablesToValuesMap: IVariableToValueEntry[];
  }

  export interface IVariableToValueEntry {
    name: string;
    value: string;
  }
}
