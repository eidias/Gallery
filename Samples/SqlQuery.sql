--<Declaration>--
DECLARE @p0 VARCHAR(32) = 'Gallery'
--</Declaration>--

--<SelectFullTextSample>--
SELECT [SelectFullTextSampleId] FROM [dbo].[SelectFullTextSample] WHERE CONTAINS(*, @p0)
--</SelectFullTextSample>--
