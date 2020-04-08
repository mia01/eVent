import Task from '../task';
import IFullCalenderEvent from '../interfaces/fullCalenderEvent.interface';
import { EventType } from '../enums/EventTypes.enum';

export default class TaskEvent implements IFullCalenderEvent {
    private task: Task;
    private type: EventType;
    private id: number;
    private title: string;
    private start: Date;
    private end: Date;
    private allDay: boolean;
    private className: string;
    
    constructor(task: Task) {
        this.task = task;
        this.type = EventType.Task;
        this.id = task.id;
        this.title = task.title;
        this.start = task.dueDate;
        this.end = task.dueDate;
        this.allDay = true;
        this.className = "full-calender-event-task"
    }
    
    // getters and setters
    public get Task(): Task {
        return this.task;
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
