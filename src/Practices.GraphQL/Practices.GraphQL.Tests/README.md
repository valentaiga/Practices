# Xunit

## Goals
- [ ] Implement unit tests with high code coverage percent

## Theory
### Fact
**Facts** are tests which are always true. They test invariant conditions.
### Theory
**Theories** are tests which are only true for a particular set of data.

## Shared context
### Class Fixtures
_When to use_: when you want to create a single test context and share it among all the tests in the class, and have it cleaned up after all the tests in the class have finished.
### Collection Fixtures
When to use: when you want to create a single test context and share it among tests in several test classes, and have it cleaned up after all the tests in the test classes have finished.
Attributes `[CollectionDefinition("Api collection")]` and `[Collection("Api collection")]` required!

