CREATE TABLE [Client]
(
	[ClientId] INTEGER NOT NULL,
	[ClientIdentifier] TEXT NOT NULL,
	[ClientSecret] TEXT NULL,
	[Callback] TEXT NULL,
	[Name] TEXT NOT NULL,
	[ClientType] INTEGER NOT NULL,
	primary key([ClientId])
)
;
CREATE TABLE [ClientAuthorization]
(
	[AuthorizationId] INTEGER NOT NULL,
	[CreatedOnUtc] DATETIME NOT NULL,
	[ClientId] INTEGER NOT NULL,
	[UserId] INTEGER NULL,
	[Scope] TEXT NULL,
	[ExpirationDateUtc] DATETIME NULL,
	primary key([AuthorizationId])
)
;
CREATE TABLE [Nonce]
(
	[Context] TEXT NOT NULL,
	[Code] TEXT NOT NULL,
	[Timestamp] TEXT NOT NULL,
	primary key([Context], [Code], [Timestamp])
)
;
CREATE TABLE [SymmetricCryptoKey]
(
	[Bucket] TEXT NOT NULL,
	[Handle] TEXT NOT NULL,
	[ExpiresUtc] DATETIME NOT NULL,
	[Secret] BLOB NOT NULL,
	primary key([Bucket], [Handle])
)
;
CREATE TABLE [User]
(
	[UserId] INTEGER NOT NULL,
	[OpenIDClaimedIdentifier] TEXT NOT NULL,
	[OpenIDFriendlyIdentifier] TEXT NULL,
	primary key([UserId])
)
;
insert into client values(1, 'sampleconsumer', 'samplesecret', null, 'Web4', 0)