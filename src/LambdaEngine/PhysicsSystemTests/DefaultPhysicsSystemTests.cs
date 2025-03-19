using System.Numerics;
using LambdaEngine.PhysicsSystem;

namespace PhysicsSystemTests;

public class DefaultPhysicsSystemTests {
    private DefaultPhysicsSystem physicsSystem;
    
    [SetUp]
    public void Setup() {
        physicsSystem = new DefaultPhysicsSystem();
        physicsSystem.Initialize(true);
    }

    [TearDown]
    public void Teardown() {
        physicsSystem.Shutdown();
    }

    #region Collider creation, destruction, initialization
    [Test]
    public void CreateBoxCollider_IncrementsId() {
        const int c0Expected = 0;
        const int c1Expected = 1;

        int c0 = physicsSystem.CreateBoxCollider();
        int c1 = physicsSystem.CreateBoxCollider();
        
        Assert.Multiple(() => {
            Assert.That(c0, Is.EqualTo(c0Expected));
            Assert.That(c1, Is.EqualTo(c1Expected));
        });
    }
    
    [Test]
    public void CreateCircleCollider_IncrementsId() {
        const int c0Expected = 0;
        const int c1Expected = 1;

        int c0 = physicsSystem.CreateCircleCollider();
        int c1 = physicsSystem.CreateCircleCollider();
        
        Assert.Multiple(() => {
            Assert.That(c0, Is.EqualTo(c0Expected));
            Assert.That(c1, Is.EqualTo(c1Expected));
        });
    }

    [Test]
    public void CreateBoxCollider_InitializesPositionWithZero() {
        int c0 = physicsSystem.CreateBoxCollider();
        
        Assert.That(physicsSystem.GetColliderPosition(c0), Is.EqualTo(Vector2.Zero));
    }
    
    [Test]
    public void CreateCircleCollider_InitializesPositionWithZero() {
        int c0 = physicsSystem.CreateCircleCollider();
        
        Assert.That(physicsSystem.GetColliderPosition(c0), Is.EqualTo(Vector2.Zero));
    }
    
    [Test]
    public void CreateBoxCollider_InitializesSizeWithOne() {
        int c0 = physicsSystem.CreateBoxCollider();
        
        Assert.Multiple(() => {
            Assert.That(physicsSystem.GetColliderWidth(c0), Is.EqualTo(1));
            Assert.That(physicsSystem.GetColliderHeight(c0), Is.EqualTo(1));
        });
    }
    
    [Test]
    public void CreateCircleCollider_InitializesRadiusWithOne() {
        int c0 = physicsSystem.CreateCircleCollider();
        
        Assert.That(physicsSystem.GetColliderRadius(c0), Is.EqualTo(1));
    }

    [Test]
    public void DestroyCollider_DestroysCollider() {
        int c0 = physicsSystem.CreateBoxCollider();
        
        physicsSystem.DestroyCollider(c0);

        Assert.Throws<KeyNotFoundException>(() => physicsSystem.GetColliderPosition(c0));
    }

    [Test]
    public void DestroyCircleCollider_KeepsOtherIdsIntact() {
        const int c1Expected = 1;
        
        int c0 = physicsSystem.CreateBoxCollider();
        int c1 = physicsSystem.CreateCircleCollider();
        
        physicsSystem.DestroyCollider(c0);
        
        Assert.Multiple(() => {
            Assert.That(c1, Is.EqualTo(c1Expected));
            Assert.DoesNotThrow(() => physicsSystem.GetColliderPosition(c1));
        });
    }
    #endregion
    
    #region Collider Getter and Setter
    [Test]
    public void SetColliderPosition_SetsPosition() {
        Vector2 c0ExpectedPos = Vector2.One;
        Vector2 c1ExpectedPos = Vector2.UnitX;

        int c0 = physicsSystem.CreateBoxCollider();
        int c1 = physicsSystem.CreateCircleCollider();
        
        physicsSystem.SetColliderPosition(c0, c0ExpectedPos);
        physicsSystem.SetColliderPosition(c1, c1ExpectedPos);
        
        Assert.Multiple(() => {
            Assert.That(physicsSystem.GetColliderPosition(c0), Is.EqualTo(c0ExpectedPos));
            Assert.That(physicsSystem.GetColliderPosition(c1), Is.EqualTo(c1ExpectedPos));
        });
    }

    [Test]
    public void SetColliderWidth_SetsWidth() {
        const float expectedWidth = 2.0f;
        
        int c0 = physicsSystem.CreateBoxCollider();
        
        physicsSystem.SetColliderWidth(c0, expectedWidth);
        
        Assert.That(physicsSystem.GetColliderWidth(c0), Is.EqualTo(expectedWidth));
    }
    
    [Test]
    public void SetColliderHeight_SetsHeight() {
        const float expectedHeight = 2.0f;
        
        int c0 = physicsSystem.CreateBoxCollider();
        
        physicsSystem.SetColliderHeight(c0, expectedHeight);
        
        Assert.That(physicsSystem.GetColliderHeight(c0), Is.EqualTo(expectedHeight));
    }
    
    [Test]
    public void SetColliderRadius_SetsRadius() {
        const float expectedRadius = 2.0f;
        
        int c0 = physicsSystem.CreateCircleCollider();
        
        physicsSystem.SetColliderRadius(c0, expectedRadius);
        
        Assert.That(physicsSystem.GetColliderRadius(c0), Is.EqualTo(expectedRadius));
    }
    #endregion
}