import { EventType } from './enums/EventTypes.enum';

export default class Event {
    private type: EventType = EventType.Event;
    id: number;
    title: string;
    description: string;
    createdByUsername: string;
    startDate: Date;
    endDate: Date;
    reminder: boolean;
    publicEvent: boolean;
}
