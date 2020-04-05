export class User {
    id: number;
    firstName: string;
    lastName: string;
    password: string;
    email: string;
    city: string;
    phone: string;
    isRegistered = false;
    userType = 'regular';

    constructor(fname: string, lname: string, password: string, email: string, city: string, phone: string) {
        this.firstName = fname;
        this.lastName = lname;
        this.email = email;
        this.password = password;
        this.phone = phone;
        this.city = city;
    }
}
