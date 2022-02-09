using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Response;
using Catalog.Host.Models.Requests;

namespace Catalog.UnitTests.Services;

public class CatalogServiceTest
{
    private readonly ICatalogService _catalogService;

    private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
    private readonly Mock<ICatalogBrandRepository> _catalogBrandRepository;
    private readonly Mock<ICharacteristicRepository> _specificationRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;

    private readonly CatalogItem _testItem = new CatalogItem()
    {
        Name = "Name",
        CharacteristicId = 1,
        Price = 1000,
        CatalogBrandId = 1,
        PictureFileName = "1.png"
    };

    private readonly CatalogBrand _testBrand = new CatalogBrand()
    {
        Author = "Name"
    };

    public CatalogServiceTest()
    {
        _catalogItemRepository = new Mock<ICatalogItemRepository>();
        _catalogBrandRepository = new Mock<ICatalogBrandRepository>();
        _specificationRepository = new Mock<ICharacteristicRepository>();
        _mapper = new Mock<IMapper>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(dbContextTransaction.Object);

        _catalogService = new CatalogService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object, _catalogBrandRepository.Object, _specificationRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = "TestName",
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogItemSuccess = new CatalogItem()
        {
            Name = "TestName"
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "TestName"
        };

        _catalogItemRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.IsAny<int?>())).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetByPageAsync(testPageSize, testPageIndex, null);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;
        var brandFilter = 10;

        _catalogItemRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.Is<int>(i => i == brandFilter))).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetByPageAsync(testPageSize, testPageIndex, null);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_Success()
    {
        // arrange
        var id = 1;

        var catalogItemSuccess = new CatalogItem()
        {
            Id = id,
            Name = _testItem.Name
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Id = id,
            Name = _testItem.Name
        };

        _catalogItemRepository.Setup(s => s.GetByIdAsync(
            It.Is<int>(i => i == id))).ReturnsAsync(catalogItemSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetByIdAsync(id);

        // assert
        result.Should().NotBeNull();
        result?.Name.Should().Be(_testItem.Name);
        result?.Id.Should().Be(id);
    }

    [Fact]
    public async Task GetByIdAsync_Failed()
    {
        // arrange
        Task<CatalogItem> testResult = null!;

        var id = 1000;

        _catalogItemRepository.Setup(s => s.GetByIdAsync(
            It.Is<int>(i => i == id))).Returns(testResult);

        // act
        var result = await _catalogService.GetByIdAsync(id);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetBrandsAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;

        var pagingPaginatedBrandsSuccess = new PaginatedItems<CatalogBrand>()
        {
            Data = new List<CatalogBrand>()
            {
                new CatalogBrand()
                {
                    Author = _testBrand.Author,
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogBrandSuccess = new CatalogBrand()
        {
            Author = _testBrand.Author
        };

        var catalogBrandDtoSuccess = new CatalogBrandDto()
        {
            Author = _testBrand.Author
        };

        _catalogItemRepository.Setup(s => s.GetBrandsAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).ReturnsAsync(pagingPaginatedBrandsSuccess);

        _mapper.Setup(s => s.Map<CatalogBrandDto>(
            It.Is<CatalogBrand>(i => i.Equals(catalogBrandSuccess)))).Returns(catalogBrandDtoSuccess);

        // act
        var result = await _catalogService.GetBrandsAsync(testPageSize, testPageIndex);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetBrandsAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _catalogItemRepository.Setup(s => s.GetBrandsAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemsResponse<CatalogBrand>>)null!);

        // act
        var result = await _catalogService.GetBrandsAsync(testPageSize, testPageIndex);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddItemAsync_Success()
    {
        // arrange
        var testResult = 1;

        _catalogItemRepository.Setup(s => s.AddItemAsync(
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.AddItemAsync(_testItem.Name, _testItem.CatalogBrandId, _testItem.Price, _testItem.CharacteristicId, _testItem.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddItemAsync_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogItemRepository.Setup(s => s.AddItemAsync(
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.AddItemAsync(_testItem.Name, _testItem.CatalogBrandId, _testItem.Price, _testItem.CharacteristicId, _testItem.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task RemoveItem_Success()
    {
        // arrange
        var id = 1;
        var testResult = "Success";

        _catalogItemRepository.Setup(s => s.RemoveItemAsync(
            It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.RemoveItemAsync(id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task RemoveItem_Failed()
    {
        // arrange
        var id = 1;
        string? testResult = null;

        _catalogItemRepository.Setup(s => s.RemoveItemAsync(
            It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.RemoveItemAsync(id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateItem_Success()
    {
        // arrange
        var id = 1;
        var testResult = "Success";

        _catalogItemRepository.Setup(s => s.UpdateItemAsync(
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.UpdateItemAsync(id, _testItem.Name, _testItem.CatalogBrandId, _testItem.Price, _testItem.CharacteristicId, _testItem.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateItem_Failed()
    {
        // arrange
        var id = 1;
        string? testResult = null;

        _catalogItemRepository.Setup(s => s.UpdateItemAsync(
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.UpdateItemAsync(id, _testItem.Name, _testItem.CatalogBrandId, _testItem.Price, _testItem.CharacteristicId, _testItem.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddBrand_Success()
    {
        // arrange
        var testResult = 1;

        _catalogBrandRepository.Setup(s => s.AddBrandAsync(
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.AddBrandAsync(_testBrand.Author);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddBrand_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogBrandRepository.Setup(s => s.AddBrandAsync(
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.AddBrandAsync(_testBrand.Author);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task RemoveBrand_Success()
    {
        // arrange
        var id = 1;
        var testResult = "Success";

        _catalogBrandRepository.Setup(s => s.RemoveBrandAsync(
            It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.RemoveBrandAsync(id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task RemoveBrand_Failed()
    {
        // arrange
        var id = 1;
        string? testResult = null;

        _catalogBrandRepository.Setup(s => s.RemoveBrandAsync(
            It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.RemoveBrandAsync(id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateBrand_Success()
    {
        // arrange
        var id = 1;
        var testResult = "Success";

        _catalogBrandRepository.Setup(s => s.UpdateBrandAsync(
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.UpdateBrandAsync(id, _testBrand.Author);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateBrand_Failed()
    {
        // arrange
        var id = 1;
        string? testResult = null;

        _catalogBrandRepository.Setup(s => s.UpdateBrandAsync(
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.UpdateBrandAsync(id, _testBrand.Author);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddCharacteristic_Success()
    {
        // arrange
        var testResult = 1;

        _specificationRepository.Setup(s => s.AddCharacteristicAsync(
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<double>(),
            It.IsAny<double>(),
            It.IsAny<string>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.AddCharacteristicAsync(id, _testCharacteristic.Publisher, _testCharacteristic.YearOfPublishing, _testSpecification.NumberOfPages, _testSpecification.Language);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddSpecification_Failed()
    {
        // arrange
        int? testResult = null;

        _specificationRepository.Setup(s => s.AddCharacteristicAsync(
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<double>(),
            It.IsAny<double>(),
            It.IsAny<string>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.AddCharacteristicAsync(id, _testCharacteristic.Publisher, _testCharacteristic.YearOfPublishing, _testSpecification.NumberOfPages, _testSpecification.Language);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task RemoveSpecification_Success()
    {
        // arrange
        var id = 1;
        var testResult = "Success";

        _specificationRepository.Setup(s => s.RemoveCharacteristicAsync(
            It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.RemoveCharacteristicAsync(id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task RemoveSpecification_Failed()
    {
        // arrange
        var id = 1;
        string? testResult = null;

        _specificationRepository.Setup(s => s.RemoveCharacteristicAsync(
            It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.RemoveCharacteristicAsync(id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateSpecification_Success()
    {
        // arrange
        var id = 1;
        var testResult = "Success";

        _specificationRepository.Setup(s => s.UpdateCharacteristicAsync(
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<double>(),
            It.IsAny<double>(),
            It.IsAny<string>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.UpdateCharacteristicAsync(id, _testCharacteristic.Publisher, _testCharacteristic.YearOfPublishing, _testSpecification.NumberOfPages, _testSpecification.Language);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateSpecification_Failed()
    {
        // arrange
        var id = 1;
        string? testResult = null;

        _specificationRepository.Setup(s => s.UpdateCharacteristicAsync(
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<double>(),
            It.IsAny<double>(),
            It.IsAny<string>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.UpdateCharacteristicAsync(id, _testCharacteristic.Publisher, _testCharacteristic.YearOfPublishing, _testSpecification.NumberOfPages, _testSpecification.Language);

        // assert
        result.Should().Be(testResult);
    }
}