# Microsoft.Extensions.DependencyInjection

!!! warning "Warning!!!"
    Support for this container is purely proof of concept. This may still have a number of bugs.

For a number of developers, the Service Provider in Microsoft.Extensions.DependencyInjection is a great lightweight D.I. container. Traditionally this has never been seriously considered for Prism support since Prism has a hard requirement on named services in order to resolve your Views. Prism also has a bit of a requirement that it be able to resolve a service that was never registered.

## What makes this different?

The requirement for named services never went away, nor did the requirement that we can resolve types like your ViewModels which are never directly registered. This package provides some additions though that allow us to track mapping between keys and types as well as to resolve concrete types even if they haven't been registered. With these two enhancements built on top of Microsoft's DI Extensions we are able to support Prism with the container.

!!! note "Backstory"
    While on the train Dan and Allan had a call and Allan said there was no way Microsoft.Extensions.DependencyInjection could work with Prism because of the requirement for named services... Dan had around 24 hours of flight time ahead of him and so he said "Challenge Accepted!"

## Using the Container

This container is most likely to be used any time that you're using Shiny, but this can be used completely independently from Shiny or even Prism.