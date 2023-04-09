import { mapEnumToOptions } from '@abp/ng.core';

export enum Status {
  Initial = 1,
  Todo = 2,
  InProgress = 3,
  Suspended = 4,
  InReview = 5,
  Done = 6,
  Cancel = 7,
}

export const statusOptions = mapEnumToOptions(Status);
