# Expression Invariant Extraction

Extract a typed object with the invariant values from a lambda expression.

## Say What?

Supposing that we have a simple type:
```
class FileKey
{
    public int? AccountId { get; set; }

    public DateTime? CreateTime { get; set; }
}
```
And given a selector-style lambda expression that evaluates to bool (like we might give to a `Where()` call):
```
k => k.AccountId == 123 && k.CreateTime > new DateTime(2012, 3, 4)
```
We can infer by the `==` in the expression that a passing `FileKey`'s `AccountId` must have an **invariant** value of 
123. 

Note that we aren't sure what `CreateTime` might be because it is an inequality instead of `==`, so it is 
**not invariant** in this sense.

You can use `InvariantExtractor` to get an instance of the simple type with all of the invariant property values 
extracted from the lambda.

## Usage

Create a new `InvariantExtractor`, and call `ExtractInvariants()` providing your lambda:
```
Expression<Func<FileKey, bool>> invariantExpression;

var fileKey = new InvariantExtractor().ExtractInvariants(
    k => k.AccountId == 123 && k.CreateTime > new DateTime(2012, 3, 4),
    out invariantExpression);
```
The `fileKey` variable will now have data like this:
```
{
    AccountId = 123,

    CreateTime = null
}
```

## Invariant Expression

Note that `ExtractInvariants()` also gives back an invariant expression.

This is identical to the expression you provide, except with the right-side values of invariants flattened to their 
constant value.

Consider that this expression will have a different value each time it is evaluated:
```
k => k.AccountId == new Random().Next()
```
The invariant expression will be created by performing a one-time evaluation of `new Random().Next()`, with the 
resultant value replacing the right-side expression:
```
k => k.AccountId == 1296692329
```

## Make Mutations of Mutable or Immutable Objects

The `Mutate` extension method allows you to create a clone of a (possibly immutable) object, 
while specifying _by example_ the values of the properties that you would like to change:
```
var original = new FileKey
{
    AccountId = 1234,

    CreateTime = new DateTime(1980, 1, 1)
};

var mutant = original.Mutate(o => o.AccountId == 2345);
```
Here `mutant` will have the updated `AccountId` of `2345`, while its `CreateTime` property will remain `1/1/1980`, 
identical to the original.