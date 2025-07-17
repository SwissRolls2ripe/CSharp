SELECT s.name AS 架构名, t.name AS 表名
FROM sys.tables t
JOIN sys.schemas s ON t.schema_id = s.schema_id;


--SurrogateIdentityId	SurrogateLockOwnerId

select * from [System.Activities.DurableInstancing].[IdentityOwnerTable]

SELECT * FROM [System.Activities.DurableInstancing].[Instances]

SELECT * FROM [System.Activities.DurableInstancing].[InstancesTable]

--DefinitionIdentityTable RunnableInstancesTable KeysTable LockOwnersTable
--InstanceMetadataChangesTable ServiceDeploymentsTable InstancePromotedPropertiesTable SqlWorkflowInstanceStoreVersionTable