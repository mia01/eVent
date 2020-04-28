import Event from '../event';
import IFullCalenderEvent from '../interfaces/fullCalenderEvent.interface';
import { EventType } from '../enums/EventTypes.enum';

export default class UserEvent implements IFullCalenderEvent {
    private event: Event;
    private type: EventType;
    private id: number;
    private title: string;
    private start: Date;
    private end: Date;
    private allDay: boolean;
    private className: string;
    
    constructor(event: Event) {
        this.event = event;
        this.type = EventType.Event;
        this.id = event.id;
        this.title = event.title;
        this.start = event.startDate;
        this.end = event.endDate;
        this.className = "full-calender-event"
    }
    
    // getters and setters
    public get Event(): Event {
        return this.event;
    }
    
    public get Type(): EventType {
        return this.type;
    }
    
    public get Id(): number {
        return this.id;
    }

    public set Id(value: number) {
        this.id = value;
    }
    
    public get Title(): string {
        return this.title;
    }
   
    public set Title(value: string) {
        this.title = value;
    }

    public get Start(): Date {
        return this.start;
    }

    public set Start(value: Date) {
        this.start = value;
    }
    
    public get End(): Date {
        return this.end;
    }

    public set End(value: Date) {
        this.end = value;
    }
    
    public get AllDay(): boolean {
        return this.allDay;
    }

    public set AllDay(value: boolean) {
        this.allDay = value;
    }

    public get ClassName(): string {
        return this.className;
    }

    public set ClassName(value: string) {
        this.className = value;
    }
}
