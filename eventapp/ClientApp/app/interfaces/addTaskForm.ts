import Priority from "../models/priority";
import Task from "../models/task";
import UserFriendResponse from '../models/UserFriendResponse';

export default class IAddTaskForm {
    taskData?: Task;
    priorities: Priority[];
    friends: UserFriendResponse[];
}
