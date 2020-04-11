export class User {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    gender: string;
    friendsList: Array<User>;
    userType: string;
    id: number;

    constructor(fname: string, lname: string, pasw: string, email: string, city: string, phone: string  ) {
        this.firstName = fname;
        this.lastName = lname;
        this.email = email;
        this.password = pasw;
        this.friendsList = new Array<User>();
    }
}
