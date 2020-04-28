export default class Task {
    id: number;
    title: string;
    description: string;
    done: boolean;
    priorityId: number;
    dueDate: Date;
    createdAt: Date;
    createdByUsername: string;
    assignedToUsername: string;
    updatedAt: Date;
    reminder: boolean;
    assignedTo: string
}
