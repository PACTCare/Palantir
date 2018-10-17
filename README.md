# About

[Read me!](https://blog.florence.chat/introducing-iota-palant%C3%ADr-messaging-ipm-c599ed6d2191)

This document has the objective to give you an overview of how to use Palantir in your applications.

# Flow

Assume you have two users, "Chantal" and "Kevin", who want to communicate through a secured channel. To set up their channel the following has to be done:

1) Create User "Kevin" and "Chantal"
2) One user has to send a contact request to the other
3) The contact request has to be accepted
4) They can now chat within their own secure channel

You can have a look at the [cucumber](https://github.com/PACTCare/Palantir/tree/master/Pact.Palantir.Cucumber/Features) tests, the [example](https://github.com/PACTCare/Palantir/tree/master/Pact.Palantir.Examples) or read the usecase descriptions to see how things are set up codewise.

# Usecases

The Messenger follows a usecase orientated approach. The code snippets for every usecase reflect how it is used in Chiota.

![cleanarch](http://i.imgur.com/WkBAATy.png)

More information:
http://blog.8thlight.com/uncle-bob/2012/08/13/the-clean-architecture.html

- [Create User](https://github.com/PACTCare/Palantir/tree/master/Documentation/Usecases/createuser.md)
- [Check User](https://github.com/PACTCare/Palantir/tree/master/Documentation/Usecases/checkuser.md)
- [Add Contact](https://github.com/PACTCare/Palantir/tree/master/Documentation/Usecases/addcontact.md)
- [Get Contacts](https://github.com/PACTCare/Palantir/tree/master/Documentation/Usecases/getcontacts.md)
- [Accept Contact](https://github.com/PACTCare/Palantir/tree/master/Documentation/Usecases/acceptcontact.md)
- [Decline Contact](https://github.com/PACTCare/Palantir/tree/master/Documentation//Usecasesdeclinecontact.md)
- [Send Message](https://github.com/PACTCare/Palantir/tree/master/Documentation/Usecases/sendmessage.md)
- [Get Messages](https://github.com/PACTCare/Palantir/tree/master/Documentation/Usecases/getmessages.md)

# Response Codes

Each response of a usecase inherits from a base response that does contain a response code. You can use the response code to display error messages to your users if needed. For further details see the ResponseCode enumeration.

# Entities

While interactors are in place to orchestrate behaviour, entities are supposed to hold it. For details, just take a look at the Entity folder. 

You may notice that there currently is not so much business logic in them. That is a thing that will be addressed iteratively, while refactoring the interactors to be pure orchestrators.

# Repositories | Cache

For most modern applications it is not possible to be completely clean of outer dependencies. To handle those, repositories have been put in place, to inverse those dependencies.

The messenger module defines two repositories you have to implement yourself (You could start off with the memory implementations for testing, but they will not get you far)

1) IContactRepository
    
    There are two options to implement this interface:<br>
    a) Implement the complete interface to use your own logic<br>
    b) Extend the AbstractTangleContactRepository, that already contains basic logic to load contacts from the tangle

2) ITransactionCache

    This repository caches all transactions received by the messenger. You should implement it in order to significantly speed up your application. In addition cached transactions can not be wiped in a snapshot, which can be useful

# Donate

See: https://ecosystem.iota.org/projects/untangle-care
