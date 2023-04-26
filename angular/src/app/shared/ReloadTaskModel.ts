export class ReloadTaskModel {
  constructor(taskId: string, senderId: string) {
    this.taskId = taskId;
    this.senderId = senderId;
  }
  taskId: string;
  senderId: string;
}
