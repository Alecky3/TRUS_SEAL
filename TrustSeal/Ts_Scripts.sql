
-- Create Sequence for Case Number

USE TrustSeal;

CREATE SEQUENCE BusinessCaseSequence
AS int
START WITH 1
INCREMENT BY 1;

go

-- Create Sequence for Seal Readable Id

USE TrustSeal;
CREATE SEQUENCE SealReadableId
AS int
START WITH 1
INCREMENT BY 1;

GO

--- Create Support Ticket Sequence
Use TrustSeal;

CREATE SEQUENCE SupportTicket
AS int
START WITH 1
INCREMENT BY 1

GO

-- Create Tracking Statuses

INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Under Review',GETDATE(),GETDATE(),1,0,1,1,1);

  INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Business Information Verified',GETDATE(),GETDATE(),1,0,1,2,1);
  INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Business Information Could Not be Verified',GETDATE(),GETDATE(),1,0,1,3,1);
  INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Business Attachments Verified',GETDATE(),GETDATE(),1,0,1,4,1);
  INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Business Attachments Could Not be Verified',GETDATE(),GETDATE(),1,0,1,5,1);
  INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Web Check Verified',GETDATE(),GETDATE(),1,0,1,6,1);
  INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Web Check Could Not be Verified',GETDATE(),GETDATE(),1,0,1,7,1);
  INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Web Check Attachments Verified',GETDATE(),GETDATE(),1,0,1,8,1);
  INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Web Check Attachments Could Not be Verified',GETDATE(),GETDATE(),1,0,1,9,1);
  ---
  INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Security Check Verified',GETDATE(),GETDATE(),1,0,1,10,1);
  INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Security Check Could Not be Verified',GETDATE(),GETDATE(),1,0,1,11,1);
  INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Security Check Attachments Verified',GETDATE(),GETDATE(),1,0,1,12,1);
  INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Security Check Attachments Could Not be Verified',GETDATE(),GETDATE(),1,0,1,13,1);
  ---
  ---
  INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Supply Chain Verified',GETDATE(),GETDATE(),1,0,1,14,1);
  INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Supply Chain Could Not be Verified',GETDATE(),GETDATE(),1,0,1,15,1);
  INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Supply Chain Attachments Verified',GETDATE(),GETDATE(),1,0,1,16,1);
  INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Supply Chain Attachments Could Not be Verified',GETDATE(),GETDATE(),1,0,1,17,1);
  ---
  INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Payment Received',GETDATE(),GETDATE(),1,0,1,18,1);

  INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Interview Done',GETDATE(),GETDATE(),1,0,1,19,1);

  INSERT INTO ApplicationTrackings (Status,CreatedAt,UpdatedAt,Required,StepVerified,NeedsAttention,[Order],IsMain)
  VALUES('Verified',GETDATE(),GETDATE(),1,0,1,20,1);


