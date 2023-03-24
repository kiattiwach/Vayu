import { mapEnumToOptions } from '@abp/ng.core';

export enum Status {
  Initial = 0,
  Todo = 1,
  InProgress = 2,
  Suspended = 3,
  InReview = 4,
  Done = 5,
  Cancel = 6,
}

export const statusOptions = mapEnumToOptions(Status);
