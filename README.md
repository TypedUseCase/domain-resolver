Domain Resolver
===============

[![NuGet](https://img.shields.io/nuget/v/Tuc.DomainResolver.svg)](https://www.nuget.org/packages/Tuc.DomainResolver)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Tuc.DomainResolver.svg)](https://www.nuget.org/packages/Tuc.DomainResolver)
[![Checks](https://github.com/TypedUseCase/domain-resolver/actions/workflows/tests.yaml/badge.svg)](https://github.com/TypedUseCase/domain-resolver/actions/workflows/tests.yaml)

> Library for resolving a **Domain types** out of a F# script (`.fsx`) file(s).

## Example

The process of this use-case is a collecting interactions by the users.

User interacts with the GenericService, which sends an interaction to the interaction collector service.
Interaction collector service identify a person and accepts an interaction.

*It is just a simplified real-life process.*

*Note: All files are in the [example](https://github.com/TypedUseCase/tuc-console/tree/master/example) dir.*

### consentsDomain.fsx
```fs
// Common types

type Id = UUID

type Stream<'Event> = Stream of 'Event list
type StreamHandler<'Event> = StreamHandler of ('Event -> unit)

// Types

type InteractionEvent =
    | Confirmation
    | Rejection

type InteractionResult =
    | Accepted
    | Error

type IdentityMatchingSet = {
    Contact: Contact
}

and Contact = {
    Email: Email option
    Phone: Phone option
}

and Email = Email of string
and Phone = Phone of string

type Person =
    | Known of PersonId
    | Unknown

and PersonId = PersonId of Id

// Streams

type InteractionCollectorStream = InteractionCollectorStream of Stream<InteractionEvent>

// Services

type GenericService = Initiator

type InteractionCollector = {
    PostInteraction: InteractionEvent -> InteractionResult
}

type PersonIdentificationEngine = {
    OnInteractionEvent: StreamHandler<InteractionEvent>
}

type PersonAggregate = {
    IdentifyPerson: IdentityMatchingSet -> Person
}

type ConsentManager = {
    GenericService: GenericService
    InteractionCollector: InteractionCollector
}
```

---
### Development

First run:
```shell
./build.sh  # or build.cmd if your OS is Windows  (might need ./build Build here)
```

Everything is done via `build.cmd` \ `build.sh` (_for later on, lets call it just `build`_).
- to run a specific target use `build -t <target>`
