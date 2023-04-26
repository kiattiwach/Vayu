import {Injectable} from '@angular/core';
import {HttpTransportType, HubConnection, HubConnectionBuilder} from "@microsoft/signalr";
import {environment} from "../../environments/environment";
import {VayuServiceService} from "./vayu-service.service";
import {ReloadTaskModel} from "../shared/ReloadTaskModel";

@Injectable({
  providedIn: 'root'
})
export class HubService {
  private hubConnection: HubConnection;
  constructor(private vayuService: VayuServiceService) { }

  init() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(environment.apis.default.url + '/MessagingHub',
        {
          skipNegotiation: true,
          transport: HttpTransportType.WebSockets
        }).build();

    this.hubConnection.on("ReloadTaskList", (...data) => {
      this.vayuService.reloadTask$.next(new ReloadTaskModel(data[0], data[1]));
    });

    this.hubConnection.on("TestSignalR", (...data) => {
      console.log(data);
    });

    this.hubConnection.start()
      .then(() => {
      })
      .catch(err => {
        console.log(`Error while establishing connection, retrying...${err} host: ${environment.apis.default.url}`);
      });
  }
}
